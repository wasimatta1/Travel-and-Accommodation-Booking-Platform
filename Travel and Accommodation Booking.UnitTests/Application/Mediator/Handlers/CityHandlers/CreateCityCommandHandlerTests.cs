using Application.Mediator.Commands.CityCommands;
using Application.Mediator.Handlers.CityHandler;
using AutoMapper;
using Contracts.DTOs.City;
using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Travel_and_Accommodation_Booking.UnitTests.Application.Mediator.Handlers.CityHandlers
{
    public class CreateCityCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<CreateCityCommandHandler>> _mockLogger;
        private readonly CreateCityCommandHandler _handler;


        public CreateCityCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<CreateCityCommandHandler>>();

            _handler = new CreateCityCommandHandler(
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task CreateCityCommandHandler_ValidCity_ReturnsCityId()
        {
            // Arrange
            var command = new CreateCityCommand
            {
                CreateCityDto = new CreateCityDto
                {
                    Name = "Name",
                    Country = "Country",
                    ThumbnailURL = "ThumbnailURL",
                    PostOffice = "PostOffice"
                }
            };

            var city = new City
            {
                CityID = 1,
                Name = command.CreateCityDto.Name,
                Country = command.CreateCityDto.Country,
                ThumbnailURL = command.CreateCityDto.ThumbnailURL,
                PostOffice = command.CreateCityDto.PostOffice
            };

            _mockMapper.Setup(m => m.Map<City>(command.CreateCityDto)).Returns(city);
            _mockUnitOfWork.Setup(u => u.Cities.CreateAsync(city)).ReturnsAsync(city);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(city.CityID, result);
            _mockMapper.Verify(m => m.Map<City>(command.CreateCityDto), Times.Once);
            _mockUnitOfWork.Verify(u => u.Cities.CreateAsync(city), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }
    }
}