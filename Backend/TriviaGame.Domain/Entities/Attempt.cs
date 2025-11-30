namespace TriviaGame.Domain.Entities;

public class Attempt
{
    public int Id { get; set; }
    public int GameSessionId { get; set; }
    public int UserId { get; set; }
    public int QuestionId { get; set; }
    public bool Correct { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}