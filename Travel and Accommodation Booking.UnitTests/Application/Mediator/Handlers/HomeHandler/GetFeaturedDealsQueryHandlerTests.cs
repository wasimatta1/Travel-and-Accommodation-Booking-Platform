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
    public class GetFeaturedDealsQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<GetFeaturedDealsQueryHandler>> _mockLogger;
        private readonly GetFeaturedDealsQueryHandler _handler;

        public GetFeaturedDealsQueryHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<GetFeaturedDealsQueryHandler>>();
            _handler = new GetFeaturedDealsQueryHandler(_mockUnitOfWork.Object, _mockLogger.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsFeaturedDeals()
        {
            // Arrange
            var request = new GetFeaturedDealsQuery
            {
                Take = 5
            };

            var rooms = new List<Room>
        {
            new Room { RoomID = 1,  PricePerNight = 150 },
            new Room { RoomID = 2, PricePerNight = 100 }
        };

            var expectedDtos = new List<FeaturedDealDto>
        {
            new FeaturedDealDto {  PricePerNight = 150, PricePerNightDiscounted = 100 },
            new FeaturedDealDto {  PricePerNight = 100, PricePerNightDiscounted = 80 }
        };

            _mockUnitOfWork.Setup(u => u.Rooms.GetFeaturedRoomsAsync(request.Take))
                .ReturnsAsync(rooms);

            _mockMapper.Setup(m => m.Map<IEnumerable<FeaturedDealDto>>(rooms))
                .Returns(expectedDtos);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDtos.Count, result.Count());
            Assert.Equal(expectedDtos[0].PricePerNight, result.First().PricePerNight);
            Assert.Equal(expectedDtos[1].PricePerNight, result.Last().PricePerNight);

            _mockUnitOfWork.Verify(u => u.Rooms.GetFeaturedRoomsAsync(request.Take), Times.Once);
        }

        [Fact]
        public async Task Handle_NoFeaturedDeals_ReturnsEmptyList()
        {
            // Arrange
            var request = new GetFeaturedDealsQuery
            {
                Take = 5
            };

            _mockUnitOfWork.Setup(u => u.Rooms.GetFeaturedRoomsAsync(request.Take))
                .ReturnsAsync(new List<Room>());

            _mockMapper.Setup(m => m.Map<IEnumerable<FeaturedDealDto>>(It.IsAny<IEnumerable<Room>>()))
                .Returns(new List<FeaturedDealDto>());

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _mockUnitOfWork.Verify(u => u.Rooms.GetFeaturedRoomsAsync(request.Take), Times.Once);
        }
    }
}