using Application.Mediator.Commands.RoomCommands;
using Application.Mediator.Handlers.RoomHandler;
using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Travel_and_Accommodation_Booking.UnitTests.Application.Mediator.Handlers.RoomHandlers
{
    public class DeleteRoomCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILogger<DeleteRoomCommandHandler>> _mockLogger;
        private readonly DeleteRoomCommandHandler _handler;

        public DeleteRoomCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<DeleteRoomCommandHandler>>();
            _handler = new DeleteRoomCommandHandler(_mockUnitOfWork.Object, _mockLogger.Object);
        }
        [Fact]
        public async Task Handle_InvalidRoomId_ReturnsZero()
        {
            // Arrange
            var roomId = 99;

            var command = new DeleteRoomCommand { RoomID = roomId };

            _mockUnitOfWork.Setup(u => u.Rooms.GetByIdAsync(roomId)).ReturnsAsync((Room)null);


            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(0, result);
            _mockUnitOfWork.Verify(u => u.Rooms.GetByIdAsync(roomId), Times.Once);
            _mockUnitOfWork.Verify(u => u.Rooms.DeleteAsync(It.IsAny<Room>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Never);
        }
        [Fact]
        public async Task Handle_ValidRoomId_DeletesRoomAndReturnsSuccess()
        {
            // Arrange
            var roomId = 1;
            var room = new Room { RoomID = roomId };

            _mockUnitOfWork.Setup(u => u.Rooms.GetByIdAsync(roomId)).ReturnsAsync(room);
            _mockUnitOfWork.Setup(u => u.Rooms.DeleteAsync(room)).Returns(Task.CompletedTask);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var command = new DeleteRoomCommand { RoomID = roomId };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(1, result);
            _mockUnitOfWork.Verify(u => u.Rooms.GetByIdAsync(roomId), Times.Once);
            _mockUnitOfWork.Verify(u => u.Rooms.DeleteAsync(room), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }
    }
}