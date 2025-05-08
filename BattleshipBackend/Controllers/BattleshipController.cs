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
        Console.WriteLine("BattleshipController initialized.");
    }

    /// <summary>
    /// Retrieves the current state of the game grid.
    /// </summary>
    /// <returns>An IActionResult containing the game grid.</returns>
    [HttpGet("grid")]
    public IActionResult GetGrid()
    {
        Console.WriteLine("GetGrid API called.");
        return Ok(_gameService.GetGrid());
    }

    /// <summary>
    /// Handles the API request to make a move in the game.
    /// </summary>
    /// <param name="request">The request containing move details.</param>
    /// <returns>An IActionResult indicating the result of the move.</returns>
    [HttpPost("move")]
    public IActionResult MakeMove([FromBody] MoveRequest request)
    {
        Console.WriteLine($"MakeMove API called with coordinates ({request.X}, {request.Y}).");
        try
        {
            var (isHit, gameStatus) = _gameService.MakePlayerMove(request.X, request.Y);
            Console.WriteLine($"Move result: {(isHit ? "Hit" : "Miss")}, GameStatus: {gameStatus}.");
            return Ok(new PlayerMoveResponse { Message = isHit ? "Hit!" : "Miss.", GameStatus = gameStatus });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in MakeMove: {ex.Message}");
            return BadRequest(new PlayerMoveResponse { Error = ex.Message, GameStatus = "Error" });
        }
    }

    /// <summary>
    /// Handles the API request to place a ship on the grid.
    /// </summary>
    /// <param name="request">The request containing ship placement details.</param>
    /// <returns>An IActionResult indicating success or failure.</returns>
    [HttpPost("placeShip")]
    public IActionResult PlaceShip([FromBody] PlaceShipRequest request)
    {
        Console.WriteLine($"PlaceShip API called with coordinates ({request.X}, {request.Y}), length {request.Length}, orientation {(request.IsHorizontal ? "horizontal" : "vertical")}.");
        try
        {
            _gameService.PlaceShip(request.X, request.Y, request.Length, request.IsHorizontal);
            Console.WriteLine("Ship placed successfully.");
            return Ok(new PlaceShipResponse { Message = "Ship placed successfully." });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in PlaceShip: {ex.Message}");
            return BadRequest(new PlaceShipResponse { Error = ex.Message });
        }
    }

    /// <summary>
    /// Handles the API request to perform an AI move.
    /// </summary>
    /// <returns>An IActionResult containing the AI move details.</returns>
    [HttpGet("aiMove")]
    public IActionResult PerformAiMove()
    {
        Console.WriteLine("PerformAiMove API called.");
        try
        {
            var (x, y, isHit, gameStatus) = _gameService.ExecuteAiMove();
            Console.WriteLine($"AI move result: Coordinates ({x}, {y}), {(isHit ? "Hit" : "Miss")}, GameStatus: {gameStatus}.");
            return Ok(new AiMoveResponse { X = x, Y = y, IsHit = isHit, GameStatus = gameStatus });
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in PerformAiMove: {ex.Message}");
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