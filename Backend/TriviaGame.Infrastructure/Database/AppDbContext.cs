using TriviaGame.Domain.Entities;
using Microsoft.EntityFrameworkCore;


namespace TriviaGame.Infrastructure
{
    
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) 
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<GameSession> GameSessions => Set<GameSession>();
        public DbSet<Streak> Streaks => Set<Streak>();
        public DbSet<Team> Teams => Set<Team>();
        public DbSet<Player> Players { get; set; }
        public DbSet<Question> Questions => Set<Question>();
        public DbSet<Attempt> Attempts => Set<Attempt>();

 


      
    }
}