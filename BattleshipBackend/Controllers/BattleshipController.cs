namespace BattleshipBackend.Controllers;

using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class BattleshipController : ControllerBase
{
    private readonly IBattleshipGameService _gameService;

    public BattleshipController(IBattleshipGameService gameService)
    {
        _gameService = gameService;
    }

    [HttpGet("grid")]
    public IActionResult GetGrid()
    {
        return Ok(_gameService.GetGrid());
    }

    [HttpPost("move")]
    public IActionResult MakeMove([FromBody] MoveRequest request)
    {
        try
        {
            var (isHit, gameStatus) = _gameService.MakePlayerMove(request.X, request.Y);
            return Ok(new PlayerMoveResponse { Message = isHit ? "Hit!" : "Miss.", GameStatus = gameStatus });
        }
        catch (Exception ex)
        {
            return BadRequest(new PlayerMoveResponse { Error = ex.Message, GameStatus = "Error" });
        }
    }

    [HttpPost("placeShip")]
    public IActionResult PlaceShip([FromBody] PlaceShipRequest request)
    {
        try
        {
            _gameService.PlaceShip(request.X, request.Y, request.Length, request.IsHorizontal);
            return Ok(new PlaceShipResponse { Message = "Ship placed successfully." });
        }
        catch (Exception ex)
        {
            return BadRequest(new PlaceShipResponse { Error = ex.Message });
        }
    }

    [HttpGet("aiMove")]
    public IActionResult PerformAiMove()
    {
        try
        {
            var (x, y, isHit, gameStatus) = _gameService.ExecuteAiMove();
            return Ok(new AiMoveResponse { X = x, Y = y, IsHit = isHit, GameStatus = gameStatus });
        }
        catch (Exception ex)
        {
            return BadRequest(new AiMoveResponse { Error = ex.Message, GameStatus = "Error" });
        }
    }
}

public class MoveRequest
{
    public int X { get; set; }
    public int Y { get; set; }
}

public class PlayerMoveResponse // Was MoveResponse, updated to reflect its specific use and new fields
{
    public string Message { get; set; }
    public string Error { get; set; }
    public string GameStatus { get; set; }
}

public class AiMoveResponse // New class for AI move responses
{
    public int X { get; set; }
    public int Y { get; set; }
    public bool IsHit { get; set; }
    public string Error { get; set; }
    public string GameStatus { get; set; }
}

public class PlaceShipRequest
{
    public int X { get; set; }
    public int Y { get; set; }
    public int Length { get; set; }
    public bool IsHorizontal { get; set; }
}

public class PlaceShipResponse
{
    public string Message { get; set; }
    public string Error { get; set; }
}