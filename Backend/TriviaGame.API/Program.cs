using Microsoft.EntityFrameworkCore;
using TriviaGame.Infrastructure;
using TriviaGame.Infrastructure.Repositories;
using TriviaGame.Domain.Repositories;
using TriviaGame.Application.State;
using TriviaGame.Application.Services;
using TriviaGame.Infrastructure.Seed;

var builder = WebApplication.CreateBuilder(args);

// Load appsettings.json
builder.Configuration.AddJsonFile(
    "appsettings.json",
    optional: true,
    reloadOnChange: true
);

// DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")
                      ?? "Data Source=Trivia.db"));

// Repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<GameSessionRepository>();
builder.Services.AddScoped<IStreakRepository, StreakRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<IQuestionRepository, QuestionRepository>();

// State + Services
builder.Services.AddSingleton<GameMemoryState>();
builder.Services.AddScoped<GameService>();

// Controllers & SignalR
builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials()
              .WithOrigins("http://localhost:3000"); // frontend URL
    });
});

// Build app
var app = builder.Build();

// -------------------------
// DATABASE MIGRATION + SEED
// -------------------------
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

    //  Make sure DB  created 
    db.Database.Migrate();

    // Seed only if empty
    QuestionSeeder.Seed(db);
}

// Middleware
app.UseRouting();
app.UseCors(); 

// Map endpoints
app.MapControllers();
app.MapHub<TriviaGame.API.Hubs.GameHub>("/gamehub");

app.Run();
