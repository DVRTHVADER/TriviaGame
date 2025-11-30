namespace TriviaGame.Domain.Entities;
using System.Text.Json;
public class Question
{
    public int Id { get; set; }
    public string Mode { get; set; } = ""; // "Algorithms" or "CyberBomb"
    public string QuestionText { get; set; } = "";
    
    public string CorrectAnswer { get; set; } = "";
    public string? Payload { get; set; } // JSON for choices, hint etc.
    public List<string> Shuffle()
    {
        if (string.IsNullOrWhiteSpace(Payload))
            return new List<string>();

        try
        {
            var array = JsonSerializer.Deserialize<List<string>>(Payload);
            if (array == null || array.Count == 0)
                return new List<string>();

            // Fisher-Yates shuffle
            Random rng = new();
            for (int i = array.Count - 1; i > 0; i--)
            {
                int j = rng.Next(i + 1);
                (array[i], array[j]) = (array[j], array[i]);
            }

            // Update the payload
            Payload = JsonSerializer.Serialize(array);

            return array;
        }
        catch
        {
            // Payload not a JSON array → return empty
            return new List<string>();
        }
    }

}