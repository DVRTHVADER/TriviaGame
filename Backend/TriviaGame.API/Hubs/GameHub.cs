using Microsoft.AspNetCore.SignalR;
using TriviaGame.Application.Services;

namespace TriviaGame.API.Hubs
{
    public class GameHub : Hub
    {
        private readonly GameService _game;

        public GameHub(GameService game)
        {
            _game = game;
        }

        public async Task SelectMode(string mode)
        {
            var session = await _game.StartGameAsync(Context.ConnectionId, mode);

            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveQuestion", new
            {
                Id = session.CurrentQuestion.Id,
                Text = session.CurrentQuestion.QuestionText,
                Answers = session.CurrentQuestion.Shuffle()
            });
        }

        public async Task SubmitAnswer(int questionId, string answer)
        {
            var (session, correct) =
                await _game.SubmitAnswerAsync(Context.ConnectionId, questionId, answer);

            await Clients.Client(Context.ConnectionId).SendAsync("UpdateStats", new
            {
                session.Score,
                session.Streak,
                Correct = correct
            });

            await Clients.Client(Context.ConnectionId).SendAsync("ReceiveQuestion", new
            {
                Id = session.CurrentQuestion.Id,
                Text = session.CurrentQuestion.QuestionText,
                Answers = session.CurrentQuestion.Shuffle()
            });
        }

        public async Task StartCyberBomb()
        {
            await _game.StartCyberBomb(Context.ConnectionId);
        }

        public async Task SubmitCyberBombAnswer(int questionId, string answer)
        {
            await _game.SubmitCyberBombAnswer(Context.ConnectionId, questionId, answer);
        }
    }
}