using Microsoft.AspNetCore.Mvc;
using TriviaGame.Domain.Repositories;
using TriviaGame.Domain.Entities;

namespace TriviaGame.API.Controllers;

[ApiController]
[Route("api/users")]
public class UserController : ControllerBase
{
    private readonly IUserRepository _users;

    public UserController(IUserRepository users) => _users = users;

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest r)
    {
        var existing = await _users.GetByUsernameAsync(r.Username);
        if (existing != null)
        {
            existing.LastLogin = DateTime.UtcNow;
            await _users.SaveChangesAsync();
            return Ok(new { existing.Id, existing.Username, existing.LastStreak, existing.TotalScore, existing.BannedUntil });
        }

        var u = new User { Username = r.Username, LastLogin = DateTime.UtcNow, LastStreak = 0, TotalScore = 0 };
        await _users.AddAsync(u);
        await _users.SaveChangesAsync();
        return Ok(new { u.Id, u.Username, u.LastStreak, u.TotalScore, u.BannedUntil });
    }
}

public record LoginRequest(string Username);