using Application.Mediator.Handlers.HomeHandler;
using Application.Mediator.Queries.HomeQueries;
using AutoMapper;
using Contracts.DTOs.Home;
using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
namespace Travel_and_Accommodation_Booking.UnitTests.Application.Mediator.Handlers.HomeHandlers
{
    public class GetTrendingDestinationsQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<GetTreandingDestinationsQueryHandler>> _mockLogger;
        private readonly GetTreandingDestinationsQueryHandler _handler;

        public GetTrendingDestinationsQueryHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<GetTreandingDestinationsQueryHandler>>();
            _handler = new GetTreandingDestinationsQueryHandler(
                _mockUnitOfWork.Object,
                _mockLogger.Object,
                _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsTrendingDestinations()
        {
            // Arrange
            var request = new GetTrendingDestinationsQuery
            {
                Take = 5
            };

            var cities = new List<City>
        {
            new City { Name = "CityName1" },
            new City { Name = "CityName2" }
        };

            var expectedDtos = new List<TrendingDestinationDto>
        {
            new TrendingDestinationDto { City = "CityName1" },
            new TrendingDestinationDto { City = "CityName2" }
        };

            _mockUnitOfWork.Setup(u => u.Cities.GetTrendingDestinationsCitiesAsync(request.Take))
                .ReturnsAsync(cities);

            _mockMapper.Setup(m => m.Map<IEnumerable<TrendingDestinationDto>>(cities))
                .Returns(expectedDtos);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDtos.Count, result.Count());
            Assert.Equal(expectedDtos[0].City, result.First().City);
            Assert.Equal(expectedDtos[1].City, result.Last().City);

            _mockUnitOfWork.Verify(u => u.Cities.GetTrendingDestinationsCitiesAsync(request.Take), Times.Once);
        }

    }
}