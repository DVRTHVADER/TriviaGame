using Microsoft.AspNetCore.SignalR;
using TriviaGame.API.Hubs;
using TriviaGame.Domain.Entities;
using TriviaGame.Domain.Repositories;
using TriviaGame.Application.State;
using TriviaGame.Infrastructure.Repositories;

namespace TriviaGame.Application.Services;

public class GameService
{
    private readonly GameMemoryState _state;
    private readonly IQuestionRepository _questions;
    private readonly GameSessionRepository _gameRepo;
    private readonly IHubContext<GameHub> _hub;

    private const int BombSeconds = 10;

    public GameService(
        GameMemoryState state,
        IQuestionRepository questions,
        GameSessionRepository gameRepo,
        IHubContext<GameHub> hub)
    {
        _state = state;
        _questions = questions;
        _gameRepo = gameRepo;
        _hub = hub;
    }

  
    public async Task<Question> GetQuestionAsync(string mode)
    {
        var q = await _questions.GetRandomAsync(mode);
        q.Shuffle();
        return q;
    }

    public async Task<PlayerSession> StartGameAsync(string connectionId, string mode)
    {
        var session = _state.StartSession(connectionId, mode);
        session.CurrentQuestion = await GetQuestionAsync(mode);
        return session;
    }

    public async Task<(PlayerSession? session, bool correct)> SubmitAnswerAsync(
        string connectionId, int questionId, string answer)
    {
        var session = _state.GetSession(connectionId);
        if (session == null)
            return (null, false);

        bool correct = session.CurrentQuestion.CorrectAnswer
            .Equals(answer.Trim(), StringComparison.OrdinalIgnoreCase);

        if (correct)
        {
            session.Score += 10;
            session.Streak++;
        }
        else
        {
            session.Streak = 0;
        }

        session.CurrentQuestion = await GetQuestionAsync(session.Mode);

        return (session, correct);
    }

    // -------------------------------------------------------------------------
    // CYBER BOMB MODE
    // -------------------------------------------------------------------------
    public async Task StartCyberBomb(string connectionId)
    {
        var session = _state.StartSession(connectionId, "CyberBomb");

        // get 5 questions
        session.QuestionStack = (await _questions.GetRandomAsync("CyberBomb", 5))
                                  .OrderBy(x => Guid.NewGuid())
                                  .ToList();

        session.CurrentIndex = 0;
        session.Score = 0;
        session.Streak = 0;
        session.IsBanned = false;

        await SendNextCyberBombQuestion(connectionId);
    }

    private async Task SendNextCyberBombQuestion(string connectionId)
    {
        var s = _state.GetSession(connectionId);
        if (s == null) return;

        if (s.CurrentIndex >= s.QuestionStack.Count)
        {
            await EndCyberBombGame(connectionId);
            return;
        }

        var q = s.QuestionStack[s.CurrentIndex];
        q.Shuffle();
        s.CurrentQuestion = q;

        await _hub.Clients.Client(connectionId).SendAsync("ReceiveCyberBombQuestion", new
        {
            q.Id,
            q.QuestionText,
            Answers = q.Shuffle(),
            Timer = BombSeconds
        });
    }

    public async Task SubmitCyberBombAnswer(string connectionId, int questionId, string answer)
    {
        var s = _state.GetSession(connectionId);
        if (s == null || s.IsBanned) return;

        bool correct = s.CurrentQuestion.CorrectAnswer
            .Equals(answer.Trim(), StringComparison.OrdinalIgnoreCase);

        if (!correct)
        {
            s.IsBanned = true;

            await _hub.Clients.Client(connectionId).SendAsync("CyberBombGameOver", new
            {
                Reason = "WrongAnswer",
                s.Score,
                s.Streak
            });

            await SaveGameResult(s);
            return;
        }

        s.Score += 10;
        s.Streak++;
        s.CurrentIndex++;

        await _hub.Clients.Client(connectionId).SendAsync("CyberBombCorrect", new
        {
            s.Score,
            s.Streak
        });

        await SendNextCyberBombQuestion(connectionId);
    }

    private async Task EndCyberBombGame(string connectionId)
    {
        var s = _state.GetSession(connectionId);
        if (s == null) return;

        await _hub.Clients.Client(connectionId).SendAsync("CyberBombComplete", new
        {
            s.Score,
            s.Streak
        });

        await SaveGameResult(s);
    }

    private async Task SaveGameResult(PlayerSession s)
    {
        var game = new GameSession
        {
            Mode = s.Mode,
            UserId = s.UserId,
            Score = s.Score,
            PlayedAt = DateTime.UtcNow
        };

        await _gameRepo.AddAsync(game);
    }
}
