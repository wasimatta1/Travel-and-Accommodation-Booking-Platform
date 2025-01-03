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
    public class UpdateCityCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<UpdateCityCommandHandler>> _loggerMock;
        private readonly UpdateCityCommandHandler _handler;

        public UpdateCityCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<UpdateCityCommandHandler>>();
            _handler = new UpdateCityCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task HandleWhen_InvalidCityId_ReturnNull()
        {
            // Arrange
            var cityId = 99;
            var updateCityDto = new UpdateCityDto { CityID = cityId };

            var command = new UpdateCityCommand { UpdateCityDto = updateCityDto };

            _unitOfWorkMock.Setup(u => u.Cities.GetByIdAsync(cityId)).ReturnsAsync((City)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Null(result);

            _unitOfWorkMock.Verify(u => u.Cities.GetByIdAsync(cityId), Times.Once);
            _mapperMock.Verify(m => m.Map(It.IsAny<UpdateCityDto>(), It.IsAny<City>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.Cities.UpdateAsync(It.IsAny<City>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Never);
        }
        [Fact]
        public async Task Handle_ValidCityId_ShouldUpdateCity()
        {
            // Arrange
            var cityId = 1;
            var updateCityDto = new UpdateCityDto
            {
                CityID = cityId,
                Name = "UpdatedCity",
                Country = "UpdatedCountry",
                ThumbnailURL = "UpdatedThumbnailURL",
                PostOffice = "UpdatedPostOffice"
            };

            var city = new City
            {
                CityID = cityId,
                Name = "OldCity",
                Country = "OldCountry",
                ThumbnailURL = "oldThumbnailURL",
                PostOffice = "OldPostOffice",
                UpdatedAt = DateTime.MinValue
            };

            _unitOfWorkMock.Setup(u => u.Cities.GetByIdAsync(cityId)).ReturnsAsync(city);
            _mapperMock.Setup(m => m.Map(updateCityDto, city));
            //_unitOfWorkMock.Setup(u => u.Cities.UpdateAsync(city)).ReturnsAsync(city);

            _mapperMock.Setup(m => m.Map<CityDto>(city)).Returns(new CityDto
            {
                Name = updateCityDto.Name,
                Country = updateCityDto.Country,
                ThumbnailURL = updateCityDto.ThumbnailURL,
                PostOffice = updateCityDto.PostOffice
            });

            var command = new UpdateCityCommand { UpdateCityDto = updateCityDto };

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updateCityDto.Name, result.Name);
            Assert.Equal(updateCityDto.Country, result.Country);
            Assert.Equal(updateCityDto.ThumbnailURL, result.ThumbnailURL);
            Assert.Equal(updateCityDto.PostOffice, result.PostOffice);

            _unitOfWorkMock.Verify(u => u.Cities.GetByIdAsync(cityId), Times.Once);
            _mapperMock.Verify(m => m.Map(updateCityDto, city), Times.Once);
            _unitOfWorkMock.Verify(u => u.Cities.UpdateAsync(city), Times.Once);
            _unitOfWorkMock.Verify(u => u.CompleteAsync(), Times.Once);
            _mapperMock.Verify(m => m.Map<CityDto>(city), Times.Once);
        }

    }
}