using TriviaGame.Domain.Entities;

namespace TriviaGame.Infrastructure.Seed;

public static class QuestionSeeder
{
    public static void Seed(AppDbContext db)
    {
        if (db.Questions.Any()) return;

        var alg = new List<Question>
        {
            new() { Mode = "Algorithms", QuestionText = "Reverse 'abcde'", CorrectAnswer = "edcba" },
            new() { Mode = "Algorithms", QuestionText = "What is output of: 2+2*2 ?", CorrectAnswer = "6" },
            new() { Mode = "Algorithms", QuestionText = "What does 'O(n)' refer to?", CorrectAnswer = "Time complexity" }
        };
        var cyber = new List<Question>
        {
            new() { Mode = "CyberBomb", QuestionText = "Which header prevents clickjacking?", CorrectAnswer = "X-Frame-Options" },
            new() { Mode = "CyberBomb", QuestionText = "Which attack uses ' OR 1=1 --'?", CorrectAnswer = "SQLi" },
            new() { Mode = "CyberBomb", QuestionText = "Which input causes script execution on page? (answer: XSS)", CorrectAnswer = "XSS" }
        };

        db.Questions.AddRange(alg);
        db.Questions.AddRange(cyber);
        db.SaveChanges();
    }
}