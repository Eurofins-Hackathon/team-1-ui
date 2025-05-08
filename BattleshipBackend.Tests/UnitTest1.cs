using Xunit;
using BattleshipBackend;

namespace BattleshipBackend.Tests
{
    public class BattleshipGameServiceTests
    {
        [Fact]
        public void PlaceShip_ShouldPlaceShipCorrectly()
        {
            // Arrange
            var gameService = new BattleshipGameService();
            int x = 0, y = 0, length = 3;
            bool isHorizontal = true;

            // Act
            bool result = gameService.PlaceShip(x, y, length, isHorizontal);

            // Assert
            Assert.True(result, "Ship should be placed successfully.");
        }

        [Fact]
        public void MakeMove_ShouldReturnHitOrMiss()
        {
            // Arrange
            var gameService = new BattleshipGameService();
            gameService.PlaceShip(0, 0, 3, true); // Place a ship

            // Act
            var result = gameService.MakeMove(0, 0);

            // Assert
            Assert.Equal("Hit", result, "Move should result in a hit.");
        }

        [Fact]
        public void MakeMove_ShouldReturnMissForEmptyCell()
        {
            // Arrange
            var gameService = new BattleshipGameService();

            // Act
            var result = gameService.MakeMove(4, 4);

            // Assert
            Assert.Equal("Miss", result, "Move should result in a miss.");
        }
    }
}
