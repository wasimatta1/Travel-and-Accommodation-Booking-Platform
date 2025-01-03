using Application.Mediator.Commands.HotelPageCommands;
using Application.Mediator.Handlers.HotelPageHandler;
using Contracts.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;

namespace Travel_and_Accommodation_Booking.UnitTests.Application.Mediator.Handlers.HotelPageHandlers
{
    public class RemoveFromCartCommandHandlerTests
    {
        private readonly Mock<ICartService> _mockCartService;
        private readonly Mock<ILogger<RemoveFromCartCommandHandler>> _mockLogger;
        private readonly RemoveFromCartCommandHandler _handler;

        public RemoveFromCartCommandHandlerTests()
        {
            _mockCartService = new Mock<ICartService>();
            _mockLogger = new Mock<ILogger<RemoveFromCartCommandHandler>>();
            _handler = new RemoveFromCartCommandHandler(_mockCartService.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_ItemExistsInCart_RemovesItemSuccessfully_ReturnsTrue()
        {
            // Arrange
            var roomId = 1;
            var request = new RemoveFromCartCommand { RoomId = roomId };

            _mockCartService.Setup(c => c.RemoveFromCart(roomId)).Returns(true);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mockCartService.Verify(c => c.RemoveFromCart(roomId), Times.Once);
        }

        [Fact]
        public async Task Handle_ItemDoesNotExistInCart_ReturnsFalse()
        {
            // Arrange
            var roomId = 1;
            var request = new RemoveFromCartCommand { RoomId = roomId };

            _mockCartService.Setup(c => c.RemoveFromCart(roomId)).Returns(false);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result);
            _mockCartService.Verify(c => c.RemoveFromCart(roomId), Times.Once);
        }
    }
}