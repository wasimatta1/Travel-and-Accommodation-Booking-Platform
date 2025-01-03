using Application.Mediator.Handlers.CityHandler;
using Application.Mediator.Queries.CityQueries;
using AutoMapper;
using Contracts.DTOs.City;
using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
namespace Travel_and_Accommodation_Booking.UnitTests.Application.Mediator.Handlers.CityHandlers
{
    public class GetCityByIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<ILogger<GetCityByIdQueryHandler>> _loggerMock;
        private readonly GetCityByIdQueryHandler _handler;

        public GetCityByIdQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _loggerMock = new Mock<ILogger<GetCityByIdQueryHandler>>();
            _handler = new GetCityByIdQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object, _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnCityDto_WhenCityExists()
        {
            // Arrange
            var cityId = 1;
            var city = new City
            {
                CityID = cityId,
                Name = "NameCity"
            };
            var cityDto = new CityDto
            {
                Name = "NameCity",
                numberOfHotels = 5
            };

            _unitOfWorkMock.Setup(u => u.Cities.GetByIdAsync(cityId)).ReturnsAsync(city);
            _mapperMock.Setup(m => m.Map<CityDto>(city)).Returns(cityDto);
            _unitOfWorkMock.Setup(u => u.Cities.CountNumberOfHotelsInCityAsync(cityId)).ReturnsAsync(5);

            var query = new GetCityByIdQuery { CityID = cityId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(cityDto.Name, result.Name);
            Assert.Equal(cityDto.numberOfHotels, result.numberOfHotels);

            _unitOfWorkMock.Verify(u => u.Cities.GetByIdAsync(cityId), Times.Once);
            _mapperMock.Verify(m => m.Map<CityDto>(city), Times.Once);
            _unitOfWorkMock.Verify(u => u.Cities.CountNumberOfHotelsInCityAsync(cityId), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnNull_WhenCityDoesNotExist()
        {
            // Arrange
            var cityId = 99;

            _unitOfWorkMock.Setup(u => u.Cities.GetByIdAsync(cityId)).ReturnsAsync((City)null);

            var query = new GetCityByIdQuery { CityID = cityId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _unitOfWorkMock.Verify(u => u.Cities.GetByIdAsync(cityId), Times.Once);
            _mapperMock.Verify(m => m.Map<CityDto>(It.IsAny<City>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.Cities.CountNumberOfHotelsInCityAsync(It.IsAny<int>()), Times.Never);
        }
    }
}