using TriviaGame.Domain.Entities;

namespace TriviaGame.Domain.Repositories;

public interface ITeamRepository
{
    Task<Team?> GetByIdAsync(int id);
    Task AddAsync(Team team);
    Task SaveChangesAsync();
}