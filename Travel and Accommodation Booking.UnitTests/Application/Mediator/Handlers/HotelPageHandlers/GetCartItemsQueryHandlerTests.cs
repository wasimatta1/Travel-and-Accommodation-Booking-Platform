using Application.Mediator.Handlers.HotelPageHandler;
using Application.Mediator.Queries.HotelPageQueries;
using Contracts.DTOs.HotelPage;
using Contracts.Interfaces;
using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Travel_and_Accommodation_Booking.UnitTests.Application.Mediator.Handlers.HotelPageHandlers
{
    public class GetCartItemsQueryHandlerTests
    {
        private readonly Mock<ICartService> _mockCartService;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ICacheService> _mockCacheService;
        private readonly Mock<ILogger<GetCartItemsQueryHandler>> _mockLogger;
        private readonly GetCartItemsQueryHandler _handler;

        public GetCartItemsQueryHandlerTests()
        {
            _mockCartService = new Mock<ICartService>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCacheService = new Mock<ICacheService>();
            _mockLogger = new Mock<ILogger<GetCartItemsQueryHandler>>();
            _handler = new GetCartItemsQueryHandler(_mockCartService.Object, _mockLogger.Object, _mockUnitOfWork.Object, _mockCacheService.Object);
        }

        [Fact]
        public async Task Handle_CartItemsAvailableWithExpiredDiscount_ReturnsCartItemsWithoutDiscont()
        {
            // Arrange
            var cartItems = new List<AddRoomToCartDto>
        {
            new AddRoomToCartDto { RoomId = 1, CheckInDate = DateTime.Now.AddDays(2), CheckOutDate = DateTime.Now.AddDays(3) }
        };

            var rooms = new List<Room>
        {
            new Room { RoomID = 1, RoomNumber = "1", PricePerNight = 100, Discounts = new List<Discount>
            {
                new Discount { DiscountPercentage = 10, StartDate = DateTime.Now.AddDays(-2), EndDate = DateTime.Now.AddDays(-1) }
            }}
        };

            var request = new GetCartItemsQuery();

            _mockCartService.Setup(c => c.GetCartItems()).Returns(cartItems);
            _mockUnitOfWork.Setup(u => u.Rooms.GetRoomsByIdsAsync(It.IsAny<List<int>>())).ReturnsAsync(rooms);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(null, result.First().DescountedTotalPrice);
            Assert.Equal(100, result.First().TotalPrice);
        }
        [Fact]
        public async Task Handle_CartItemsAvailableNoDiscountHistory_ReturnsCartItemsWithoutDiscount()
        {
            // Arrange
            var cartItems = new List<AddRoomToCartDto>
        {
            new AddRoomToCartDto { RoomId = 1, CheckInDate = DateTime.Now.AddDays(2), CheckOutDate = DateTime.Now.AddDays(3) }
        };

            var rooms = new List<Room>
        {
            new Room { RoomID = 1, RoomNumber = "1", PricePerNight = 100, Discounts = new List<Discount>() }
        };

            var request = new GetCartItemsQuery();

            _mockCartService.Setup(c => c.GetCartItems()).Returns(cartItems);
            _mockUnitOfWork.Setup(u => u.Rooms.GetRoomsByIdsAsync(It.IsAny<List<int>>())).ReturnsAsync(rooms);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal(null, result.First().DescountedTotalPrice);
            Assert.Equal(100, result.First().TotalPrice);
        }
        [Fact]
        public async Task Handle_NoCartItems_ReturnsEmptyList()
        {
            // Arrange
            var cartItems = new List<AddRoomToCartDto>(); // No items in cart
            var request = new GetCartItemsQuery();

            _mockCartService.Setup(c => c.GetCartItems()).Returns(cartItems);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);
        }


        [Fact]
        public async Task Handle_ApplyDiscountToCartItems_ReturnsDiscountedPrices()
        {
            // Arrange
            var cartItems = new List<AddRoomToCartDto>
        {
            new AddRoomToCartDto { RoomId = 1, CheckInDate = DateTime.Now, CheckOutDate = DateTime.Now.AddDays(3) }
        };

            var rooms = new List<Room>
        {
            new Room
            {
                RoomID = 1,
                RoomNumber = "1",
                PricePerNight = 150,
                Discounts = new List<Discount>
                {
                    new Discount { DiscountPercentage = 20, StartDate = DateTime.Now.AddDays(-1), EndDate = DateTime.Now.AddDays(1) }
                }
            }
        };

            var request = new GetCartItemsQuery();

            _mockCartService.Setup(c => c.GetCartItems()).Returns(cartItems);
            _mockUnitOfWork.Setup(u => u.Rooms.GetRoomsByIdsAsync(It.IsAny<List<int>>())).ReturnsAsync(rooms);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            var discountedPrice = 150 * 0.8m * 3;
            Assert.Equal(discountedPrice, result.First().DescountedTotalPrice);
        }
    }
}