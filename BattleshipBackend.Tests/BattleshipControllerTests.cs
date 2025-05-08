using Xunit;
using Moq;
using Microsoft.AspNetCore.Mvc;
using BattleshipBackend.Controllers;

public class BattleshipControllerTests
{
    private readonly Mock<BattleshipGameService> _mockGameService;
    private readonly BattleshipController _controller;

    public BattleshipControllerTests()
    {
        _mockGameService = new Mock<BattleshipGameService>();
        _controller = new BattleshipController(_mockGameService.Object);
    }

    [Fact]
    public void GetGrid_ReturnsOkResultWithGrid()
    {
        // Arrange
        var mockGrid = new char[7, 7];
        _mockGameService.Setup(service => service.GetGrid()).Returns(mockGrid);

        // Act
        var result = _controller.GetGrid();

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(mockGrid, okResult.Value);
    }

    [Fact]
    public void MakeMove_ValidMove_ReturnsOkResult()
    {
        // Arrange
        var moveRequest = new MoveRequest { X = 1, Y = 1 };
        _mockGameService.Setup(service => service.MakeMove(moveRequest.X, moveRequest.Y)).Returns(true);

        // Act
        var result = _controller.MakeMove(moveRequest);

        // Assert
        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("Move successful.", ((dynamic)okResult.Value).Message);
    }

    [Fact]
    public void MakeMove_InvalidMove_ReturnsBadRequest()
    {
        // Arrange
        var moveRequest = new MoveRequest { X = 10, Y = 10 }; // Out of bounds
        _mockGameService.Setup(service => service.MakeMove(moveRequest.X, moveRequest.Y)).Throws(new ArgumentOutOfRangeException());

        // Act
        var result = _controller.MakeMove(moveRequest);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        Assert.NotNull(((dynamic)badRequestResult.Value).Error);
    }
}