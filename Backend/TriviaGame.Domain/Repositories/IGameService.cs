// TriviaGame.Application/Interfaces/IGameService.cs
using TriviaGame.Domain.Entities;


namespace TriviaGame.Application.Interfaces
{
    public interface IGameService
    {
        GameSession StartGame(List<Player> players);
        bool ValidateAnswer(Player player, string answer, string correctAnswer);
        int CalculateScore(Player player, int streak);
        void EndGame(GameSession session);
        GameSession GetGameSession(Guid sessionId);
    }
}