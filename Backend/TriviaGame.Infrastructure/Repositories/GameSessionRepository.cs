using TriviaGame.Domain.Entities;

using Microsoft.EntityFrameworkCore;


namespace TriviaGame.Infrastructure.Repositories
{
    public class GameSessionRepository
    {
        private readonly AppDbContext _context;

        public GameSessionRepository(AppDbContext context)
        {
            _context = context;
        }

        // -------------------------
        // GET ONE
        // -------------------------
        public async Task<GameSession?> GetByIdAsync(int id)
        {
            return await _context.GameSessions
                .Include(gs => gs.Players)
                .Include(gs => gs.User)
                .FirstOrDefaultAsync(gs => gs.Id == id);
        }

        // -------------------------
        // GET ALL
        // -------------------------
        public async Task<List<GameSession>> GetAllAsync()
        {
            return await _context.GameSessions
                .Include(gs => gs.Players)
                .Include(gs => gs.User)
                .ToListAsync();
        }

        // -------------------------
        // CREATE
        // -------------------------
        public async Task AddAsync(GameSession session)
        {
            await _context.GameSessions.AddAsync(session);
            await _context.SaveChangesAsync();
        }

        // -------------------------
        // UPDATE
        // -------------------------
        public async Task UpdateAsync(GameSession session)
        {
            _context.GameSessions.Update(session);
            await _context.SaveChangesAsync();
        }
    }
}
