using TriviaGame.Domain.Entities;

namespace TriviaGame.Application.State
{
    public class GameMemoryState
    {
        private readonly Dictionary<string, PlayerSession> _sessions = new();

        public PlayerSession StartSession(string connectionId, string mode)
        {
            var session = new PlayerSession
            {
                ConnectionId = connectionId,
                Mode = mode,
                Score = 0,
                Streak = 0,
            };

            _sessions[connectionId] = session;
            return session;
        }

        public PlayerSession? GetSession(string connectionId)
        {
            return _sessions.TryGetValue(connectionId, out var session) ? session : null;
        }
    }

    public class PlayerSession
    {
        public string ConnectionId { get; set; }
        public User UserId { get; set; }
        public string Mode { get; set; }

        public int Score { get; set; }
        public int Streak { get; set; }

        public List<Question> QuestionStack { get; set; } = new();
        public int CurrentIndex { get; set; }
        public Question? CurrentQuestion { get; set; }

        public bool IsBanned { get; set; }
    }
}