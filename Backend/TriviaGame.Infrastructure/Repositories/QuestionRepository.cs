using Microsoft.EntityFrameworkCore;
using TriviaGame.Domain.Entities;
using TriviaGame.Domain.Repositories;

namespace TriviaGame.Infrastructure.Repositories;

public class QuestionRepository : IQuestionRepository
{
    private readonly AppDbContext _db;
    private readonly Random _rnd = new();

    public QuestionRepository(AppDbContext db) => _db = db;

    public async Task<Question?> GetByIdAsync(int id) =>
        await _db.Questions.FindAsync(id);

    public async Task<Question> GetRandomAsync(string mode)
    {
        var list = await _db.Questions.Where(q => q.Mode == mode).ToListAsync();
        if (!list.Any()) throw new InvalidOperationException("No questions for mode " + mode);
        return list[_rnd.Next(list.Count)];
    }

    public async Task<IEnumerable<Question>> GetAllByModeAsync(string mode) =>
        await _db.Questions.Where(q => q.Mode == mode).ToListAsync();

    public async Task AddAsync(Question q) => await _db.Questions.AddAsync(q);

    public Task SaveChangesAsync() => _db.SaveChangesAsync();
}