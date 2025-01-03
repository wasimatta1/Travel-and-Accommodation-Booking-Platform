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
    public class SearchRequestQureyHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<SearchRequestQureyHandler>> _mockLogger;
        private readonly SearchRequestQureyHandler _handler;

        public SearchRequestQureyHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<SearchRequestQureyHandler>>();
            _handler = new SearchRequestQureyHandler(_mockUnitOfWork.Object, _mockLogger.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsSearchResults()
        {
            // Arrange
            var request = new SearchRequestQurey
            {
                Query = "Query",
                StarRating = 5,
                PageNumber = 1,
                PageSize = 10,
                Amenities = new string[] { "Amenity1", "Amenity2" },
                CheckInDate = DateTime.Now,
                CheckOutDate = DateTime.Now.AddDays(1),
                Adults = 2,
                Children = 0,
                Rooms = 1,
            };

            var hotels = new List<Hotel>
        {
            new Hotel { HotelID = 1, Name = "HotelName1" },
            new Hotel { HotelID = 2, Name = "HotelName2" }
        };

            var expectedDtos = new List<SearchResultDto>
        {
            new SearchResultDto {HotelName = "HotelName1" },
            new SearchResultDto {HotelName = "HotelName2"}
        };

            _mockUnitOfWork.Setup(u => u.Hotels.SearchHotels(
                request.Query, request.StarRating, request.PageNumber, request.PageSize,
                request.Amenities, request.CheckInDate, request.CheckOutDate, request.Adults, request.Children, request.Rooms,
                null, null, null))
                .ReturnsAsync(hotels);

            _mockMapper.Setup(m => m.Map<IEnumerable<SearchResultDto>>(hotels)).Returns(expectedDtos);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDtos.Count, result.Count());
        }
    }
}