using TriviaGame.Domain.Entities;

namespace TriviaGame.Domain.Repositories;

public interface IStreakRepository
{
    Task<IEnumerable<Streak>> GetByUserIdAsync(int userId);
    Task AddAsync(Streak streak);
    Task SaveChangesAsync();
}