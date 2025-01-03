using Application.Mediator.Commands.HotelCommands;
using Application.Mediator.Handlers.HotelHandler;
using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Travel_and_Accommodation_Booking.UnitTests.Application.Mediator.Handlers.HotelHandlers
{
    public class DeleteHotelCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ILogger<DeleteHotelCommandHandler>> _mockLogger;
        private readonly DeleteHotelCommandHandler _handler;

        public DeleteHotelCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockLogger = new Mock<ILogger<DeleteHotelCommandHandler>>();
            _handler = new DeleteHotelCommandHandler(_mockUnitOfWork.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_HotelDoesNotExist_ReturnsZero()
        {
            // Arrange
            var hotelId = 1;

            _mockUnitOfWork.Setup(u => u.Hotels.GetByIdAsync(hotelId)).ReturnsAsync((Hotel)null);

            var command = new DeleteHotelCommand { HotelID = hotelId };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(0, result);
            _mockUnitOfWork.Verify(u => u.Hotels.DeleteAsync(It.IsAny<Hotel>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_HotelExists_DeletesHotelAndReturnsCompletionResult()
        {
            // Arrange
            var hotelId = 1;
            var hotel = new Hotel { HotelID = hotelId };

            _mockUnitOfWork.Setup(u => u.Hotels.GetByIdAsync(hotelId)).ReturnsAsync(hotel);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var command = new DeleteHotelCommand { HotelID = hotelId };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(1, result);
            _mockUnitOfWork.Verify(u => u.Hotels.DeleteAsync(hotel), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

    }
}