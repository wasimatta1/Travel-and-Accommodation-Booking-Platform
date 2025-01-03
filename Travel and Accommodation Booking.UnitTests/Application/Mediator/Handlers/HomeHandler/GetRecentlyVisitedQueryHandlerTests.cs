using Application.Mediator.Handlers.HomeHandler;
using Application.Mediator.Queries.HomeQueries;
using AutoMapper;
using Contracts.DTOs.Home;
using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;
namespace Travel_and_Accommodation_Booking.UnitTests.Application.Mediator.Handlers.HomeHandlers
{
    public class GetRecentlyVisitedQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<GetRecentlyVisitedQueryHandler>> _mockLogger;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly GetRecentlyVisitedQueryHandler _handler;

        public GetRecentlyVisitedQueryHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<GetRecentlyVisitedQueryHandler>>();
            _mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(),
                null, null, null, null, null, null, null, null);
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _handler = new GetRecentlyVisitedQueryHandler(
                _mockUnitOfWork.Object,
                _mockLogger.Object,
                _mockMapper.Object,
                _mockUserManager.Object,
                _mockHttpContextAccessor.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsRecentlyVisitedBookings()
        {
            // Arrange
            var request = new GetRecentlyVisitedQuery
            {
                Take = 5
            };

            var user = new User { Id = "UserID", Email = "Email@example.com" };
            var bookings = new List<Booking>
        {
            new Booking { BookingID = 1 },
            new Booking { BookingID = 2 }
        };

            var expectedDtos = new List<RecentlyVisitedDto>
        {
            new RecentlyVisitedDto { HotelName = "HotelName1" },
            new RecentlyVisitedDto { HotelName = "HotelName2" }
        };

            _mockHttpContextAccessor.Setup(h => h.HttpContext.User.Claims)
                .Returns(new List<Claim>
                {
                new Claim(ClaimTypes.Email, user.Email)
                });

            _mockUserManager.Setup(u => u.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _mockUnitOfWork.Setup(u => u.Bookings.GetBookingsByUserIdAsync(user.Id, request.Take))
                .ReturnsAsync(bookings);

            _mockMapper.Setup(m => m.Map<IEnumerable<RecentlyVisitedDto>>(bookings))
                .Returns(expectedDtos);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedDtos.Count, result.Count());
            Assert.Equal(expectedDtos[0].HotelName, result.First().HotelName);
            Assert.Equal(expectedDtos[1].HotelName, result.Last().HotelName);

            _mockHttpContextAccessor.Verify(h => h.HttpContext.User.Claims, Times.Once);
            _mockUserManager.Verify(u => u.FindByEmailAsync(user.Email), Times.Once);
            _mockUnitOfWork.Verify(u => u.Bookings.GetBookingsByUserIdAsync(user.Id, request.Take), Times.Once);
        }

        [Fact]
        public async Task Handle_NoBookings_ReturnsEmptyList()
        {
            // Arrange
            var request = new GetRecentlyVisitedQuery
            {
                Take = 5
            };

            var user = new User { Id = "UserID", Email = "Email@example.com" };

            _mockHttpContextAccessor.Setup(h => h.HttpContext.User.Claims)
                .Returns(new List<Claim>
                {
                new Claim(ClaimTypes.Email, user.Email)
                });

            _mockUserManager.Setup(u => u.FindByEmailAsync(user.Email))
                .ReturnsAsync(user);

            _mockUnitOfWork.Setup(u => u.Bookings.GetBookingsByUserIdAsync(user.Id, request.Take))
                .ReturnsAsync(new List<Booking>());

            _mockMapper.Setup(m => m.Map<IEnumerable<RecentlyVisitedDto>>(It.IsAny<IEnumerable<Booking>>()))
                .Returns(new List<RecentlyVisitedDto>());

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _mockUnitOfWork.Verify(u => u.Bookings.GetBookingsByUserIdAsync(user.Id, request.Take), Times.Once);
        }
    }
}