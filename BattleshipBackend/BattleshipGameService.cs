using System;
using System.Collections.Generic;

public interface IBattleshipGameService
{
    (bool isHit, string gameStatus) MakePlayerMove(int x, int y);
    string[][] GetGrid();
    bool PlaceShip(int x, int y, int length, bool isHorizontal);
    (int x, int y, bool isHit, string gameStatus) ExecuteAiMove();
}

public class BattleshipGameService : IBattleshipGameService
{
    private const int GridSize = 5;
    private readonly char[,] _playerGrid;
    private readonly char[,] _aiGrid;
    private readonly HashSet<string> _playerHits;
    private readonly HashSet<string> _aiHits;
    private bool _isPlayerTurn = true;

    private int _playerInitialShipSegments;
    private int _aiInitialShipSegments;
    private int _playerHitShipSegments;
    private int _aiHitShipSegments;

    public BattleshipGameService()
    {
        _playerGrid = new char[GridSize, GridSize];
        _aiGrid = new char[GridSize, GridSize];
        _playerHits = new HashSet<string>();
        _aiHits = new HashSet<string>();

        _playerInitialShipSegments = 0;
        _aiInitialShipSegments = 0;
        _playerHitShipSegments = 0;
        _aiHitShipSegments = 0;

        InitializeGrid(_playerGrid);
        InitializeGrid(_aiGrid);
        PlaceAiShips();
    }

    /// <summary>
    /// Initializes the game grid with empty cells.
    /// </summary>
    /// <param name="grid">The grid to initialize.</param>
    private void InitializeGrid(char[,] grid)
    {
        for (int i = 0; i < GridSize; i++)
        {
            for (int j = 0; j < GridSize; j++)
            {
                grid[i, j] = '-'; // Empty cell
            }
        }
    }

    /// <summary>
    /// Places AI ships randomly on the grid.
    /// </summary>
    private void PlaceAiShips()
    {
        Random random = new Random();
        int[] shipLengths = { 2, 3 }; // Adjusted ship lengths for 5x5 grid

        foreach (int length in shipLengths)
        {
            bool placed = false;
            while (!placed)
            {
                int x = random.Next(0, GridSize);
                int y = random.Next(0, GridSize);
                bool isHorizontal = random.Next(0, 2) == 0;

                if (CanPlaceShip(_aiGrid, x, y, length, isHorizontal))
                {
                    PlaceShipOnGrid(_aiGrid, x, y, length, isHorizontal);
                    _aiInitialShipSegments += length; // Track AI ship segments
                    placed = true;
                }
            }
        }
    }

    /// <summary>
    /// Checks if a ship can be placed at the specified location.
    /// </summary>
    /// <param name="grid">The grid to check.</param>
    /// <param name="x">The starting x-coordinate.</param>
    /// <param name="y">The starting y-coordinate.</param>
    /// <param name="length">The length of the ship.</param>
    /// <param name="isHorizontal">Whether the ship is placed horizontally.</param>
    /// <returns>True if the ship can be placed, otherwise false.</returns>
    private bool CanPlaceShip(char[,] grid, int x, int y, int length, bool isHorizontal)
    {
        for (int i = 0; i < length; i++)
        {
            int newX = isHorizontal ? x : x + i;
            int newY = isHorizontal ? y + i : y;

            if (newX >= GridSize || newY >= GridSize || grid[newX, newY] != '-')
            {
                return false;
            }
        }
        return true;
    }

    /// <summary>
    /// Places a ship on the grid at the specified location.
    /// </summary>
    /// <param name="grid">The grid to place the ship on.</param>
    /// <param name="x">The starting x-coordinate.</param>
    /// <param name="y">The starting y-coordinate.</param>
    /// <param name="length">The length of the ship.</param>
    /// <param name="isHorizontal">Whether the ship is placed horizontally.</param>
    private void PlaceShipOnGrid(char[,] grid, int x, int y, int length, bool isHorizontal)
    {
        for (int i = 0; i < length; i++)
        {
            int newX = isHorizontal ? x : x + i;
            int newY = isHorizontal ? y + i : y;
            grid[newX, newY] = 'S'; // Mark as ship
        }
    }

    public virtual bool MakeMove(int x, int y)
    {
        if (!_isPlayerTurn)
        {
            Console.WriteLine("AI move skipped: It's not the player's turn.");
            return false; // Placeholder, will be removed when signature changes
        }

        Console.WriteLine($"Player move at ({x}, {y})");
        bool isHit = ProcessMove(_aiGrid, x, y);
        _isPlayerTurn = false; // Switch to AI's turn
        Console.WriteLine("Switching to AI's turn.");
        return isHit; // Placeholder, will be (isHit, gameStatus)
    }

    public virtual (bool isHit, string gameStatus) MakePlayerMove(int x, int y)
    {
        if (!_isPlayerTurn)
        {
            throw new InvalidOperationException("It's not the player's turn.");
        }

        Console.WriteLine($"Player move at ({x}, {y})");
        bool isHit = ProcessMove(_aiGrid, x, y);
        string gameStatus = "Ongoing";

        if (_aiInitialShipSegments > 0 && _aiHitShipSegments == _aiInitialShipSegments)
        {
            gameStatus = "Player Wins!";
        }

        if (gameStatus == "Ongoing")
        {
            _isPlayerTurn = false; // Switch to AI's turn only if game is ongoing
            Console.WriteLine("Switching to AI's turn.");
        }
        else
        {
            Console.WriteLine("Game Over: Player Wins!");
        }
        return (isHit, gameStatus);
    }

    public virtual (int x, int y, bool isHit) PerformAiMove()
    {
        if (_isPlayerTurn)
        {
            Console.WriteLine("Invalid AI move: It's not the AI's turn.");
            throw new InvalidOperationException("It's not the AI's turn.");
        }

        Random random = new Random();
        int x, y;
        do
        {
            x = random.Next(0, GridSize);
            y = random.Next(0, GridSize);
        } while (_playerGrid[x, y] == 'X' || _playerGrid[x, y] == 'O');

        Console.WriteLine($"AI move at ({x}, {y})");
        bool isHit = ProcessMove(_playerGrid, x, y);
        Console.WriteLine($"AI move result: {(isHit ? "Hit" : "Miss")}");
        _isPlayerTurn = true; // Switch back to player's turn
        return (x, y, isHit);
    }

    public virtual (int x, int y, bool isHit, string gameStatus) ExecuteAiMove()
    {
        if (_isPlayerTurn)
        {
            throw new InvalidOperationException("It's not the AI's turn.");
        }

        Random random = new Random();
        int x, y;
        do
        {
            x = random.Next(0, GridSize);
            y = random.Next(0, GridSize);
        } while (_playerGrid[x, y] == 'X' || _playerGrid[x, y] == 'O'); // Check player's grid for AI's target

        Console.WriteLine($"AI move at ({x}, {y})");
        bool isHit = ProcessMove(_playerGrid, x, y); // AI hits player's grid
        Console.WriteLine($"AI move result: {(isHit ? "Hit" : "Miss")}");
        string gameStatus = "Ongoing";

        if (_playerInitialShipSegments > 0 && _playerHitShipSegments == _playerInitialShipSegments)
        {
            gameStatus = "AI Wins!";
        }
        
        if (gameStatus == "Ongoing")
        {
            _isPlayerTurn = true; // Switch back to player's turn
        }
        else
        {
            Console.WriteLine("Game Over: AI Wins!");
        }
        return (x, y, isHit, gameStatus);
    }

    /// <summary>
    /// Processes a move on the target grid.
    /// </summary>
    /// <param name="targetGrid">The grid to process the move on.</param>
    /// <param name="x">The x-coordinate of the move.</param>
    /// <param name="y">The y-coordinate of the move.</param>
    /// <returns>True if the move is a hit, otherwise false.</returns>
    private bool ProcessMove(char[,] targetGrid, int x, int y)
    {
        if (x < 0 || x >= GridSize || y < 0 || y >= GridSize)
        {
            throw new ArgumentOutOfRangeException("Move is out of bounds.");
        }

        if (targetGrid[x, y] == 'S')
        {
            targetGrid[x, y] = 'X'; // Mark as hit
            if (targetGrid == _aiGrid) // Player is attacking AI grid
            {
                _aiHitShipSegments++;
                Console.WriteLine($"AI ship segment hit. Total AI hits: {_aiHitShipSegments}/{_aiInitialShipSegments}");
            }
            else if (targetGrid == _playerGrid) // AI is attacking player grid
            {
                _playerHitShipSegments++;
                Console.WriteLine($"Player ship segment hit. Total Player hits: {_playerHitShipSegments}/{_playerInitialShipSegments}");
            }
            return true;
        }
        else if (targetGrid[x, y] == '-')
        {
            targetGrid[x, y] = 'O'; // Mark as miss
            return false;
        }

        // Cell already targeted (X or O)
        Console.WriteLine("Cell already targeted, no changes made.");
        return false; // No-op for already targeted cells
    }

    /// <summary>
    /// Places a ship on the player's grid.
    /// </summary>
    /// <param name="x">The starting x-coordinate.</param>
    /// <param name="y">The starting y-coordinate.</param>
    /// <param name="length">The length of the ship.</param>
    /// <param name="isHorizontal">Whether the ship is placed horizontally.</param>
    /// <returns>True if the ship is placed successfully.</returns>
    public virtual bool PlaceShip(int x, int y, int length, bool isHorizontal)
    {
        Console.WriteLine($"Attempting to place ship at ({x}, {y}) with length {length} {(isHorizontal ? "horizontally" : "vertically")}.");
        if (x < 0 || y < 0 || x >= GridSize || y >= GridSize)
        {
            Console.WriteLine("Ship placement is out of bounds.");
            throw new ArgumentOutOfRangeException("Ship placement is out of bounds.");
        }

        if (!CanPlaceShip(_playerGrid, x, y, length, isHorizontal))
        {
            Console.WriteLine("Invalid ship placement. Ships overlap or are out of bounds.");
            throw new InvalidOperationException("Invalid ship placement. Ships overlap or are out of bounds.");
        }

        PlaceShipOnGrid(_playerGrid, x, y, length, isHorizontal);
        _playerInitialShipSegments += length; // Track player ship segments
        Console.WriteLine($"Player placed ship. Total player segments: {_playerInitialShipSegments}");
        return true;
    }

    public virtual string[][] GetGrid()
    {
        int rows = _playerGrid.GetLength(0);
        int cols = _playerGrid.GetLength(1);
        string[][] result = new string[rows][];

        for (int i = 0; i < rows; i++)
        {
            result[i] = new string[cols];
            for (int j = 0; j < cols; j++)
            {
                result[i][j] = _playerGrid[i, j].ToString();
            }
        }

        return result;
    }
}