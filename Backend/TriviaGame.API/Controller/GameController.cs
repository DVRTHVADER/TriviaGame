using Microsoft.AspNetCore.Mvc;
using TriviaGame.Application.Services;

namespace TriviaGame.API.Controllers;

[ApiController]
[Route("api/game")]
public class GameController : ControllerBase
{
    private readonly GameService _game;

    public GameController(GameService game) => _game = game;

    [HttpGet("question")]
    public async Task<IActionResult> GetQuestion([FromQuery] string mode = "Algorithms")
    {
        var q = await _game.GetQuestionAsync(mode);
        return Ok(new { q.Id, q.QuestionText, q.Payload });
    }
}