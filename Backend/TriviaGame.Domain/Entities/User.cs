namespace TriviaGame.Domain.Entities;


    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = "";
        public int LastStreak { get; set; }
        public DateTime LastLogin { get; set; }
        public long? BannedUntil { get; set; }
        public int TotalScore { get; set; }
    }

