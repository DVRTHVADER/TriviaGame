namespace TriviaGame.Domain.Entities
{
    public class GameSession
    {
        public int Id { get; set; }
        public string Mode { get; set; } = "";
        public int UserId { get; set; }
        public User? User { get; set; }
        public int Score { get; set; }
        public DateTime PlayedAt { get; set; }
        public ICollection<Player> Players { get; set; } = new List<Player>();
    }
}