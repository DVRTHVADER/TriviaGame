using Microsoft.EntityFrameworkCore;
using TriviaGame.Domain.Entities;
using TriviaGame.Domain.Repositories;

namespace TriviaGame.Infrastructure.Repositories;

public class StreakRepository : IStreakRepository
{
    private readonly AppDbContext _context;

    public StreakRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Streak>> GetByUserIdAsync(int userId)
        => await _context.Streaks
            .Where(s => s.UserId == userId)
            .ToListAsync();

    public async Task AddAsync(Streak streak)
        => await _context.Streaks.AddAsync(streak);

    public Task SaveChangesAsync()
        => _context.SaveChangesAsync();
}