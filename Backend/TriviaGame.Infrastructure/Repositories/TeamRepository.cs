using Microsoft.EntityFrameworkCore;
using TriviaGame.Domain.Entities;
using TriviaGame.Domain.Repositories;

namespace TriviaGame.Infrastructure.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly AppDbContext _context;

    public TeamRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<Team?> GetByIdAsync(int id)
        => await _context.Teams.FindAsync(id);

    public async Task AddAsync(Team team)
        => await _context.Teams.AddAsync(team);

    public Task SaveChangesAsync()
        => _context.SaveChangesAsync();
}