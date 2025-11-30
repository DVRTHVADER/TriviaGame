namespace TriviaGame.Domain.Entities
{
    public class Team
    {
        public int Id { get; set; }
        public string TeamName { get; set; } = "";
        public int Player1Id { get; set; }
        public int Player2Id { get; set; }
    }
}