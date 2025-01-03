using Application.Mediator.Commands.CityCommands;
using Application.Mediator.Handlers.CityHandler;
using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
namespace Travel_and_Accommodation_Booking.UnitTests.Application.Mediator.Handlers.CityHandlers
{
    public class DeleteCityCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<DeleteCityCommandHandler>> _loggerMock;
        private readonly DeleteCityCommandHandler _handler;

        public DeleteCityCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<DeleteCityCommandHandler>>();
            _handler = new DeleteCityCommandHandler(
                _unitOfWorkMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnZero_WhenCityDoesNotExist()
        {
            // Arrange
            var cityId = 99;

            _unitOfWorkMock.Setup(u => u.Cities.GetByIdAsync(cityId)).ReturnsAsync((City)null);

            var command = new DeleteCityCommand { CityID = cityId };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(0, result);
            _unitOfWorkMock.Verify(u => u.Cities.GetByIdAsync(cityId), Times.Once);
            _unitOfWorkMock.Verify(u => u.Cities.DeleteAsync(It.IsAny<City>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldDeleteCity_WhenCityExists()
        {
            // Arrange
            var cityId = 1;
            var city = new City { CityID = cityId };

            _unitOfWorkMock.Setup(u => u.Cities.GetByIdAsync(cityId)).ReturnsAsync(city);
            _unitOfWorkMock.Setup(u => u.Cities.DeleteAsync(city)).Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            var command = new DeleteCityCommand { CityID = cityId };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(1, result);
            _unitOfWorkMock.Verify(u => u.Cities.GetByIdAsync(cityId), Times.Once);
            _unitOfWorkMock.Verify(u => u.Cities.DeleteAsync(city), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
        }
    }
}