using Application.Mediator.Commands.CheckoutCommands;
using Application.Mediator.Handlers.CheckoutHandler;
using AutoMapper;
using Contracts.DTOs.Cash;
using Contracts.DTOs.Checkout;
using Contracts.Interfaces;
using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using System.Security.Claims;

namespace Travel_and_Accommodation_Booking.UnitTests.Application.Mediator.Handlers.CheckoutHadnlers
{
    public class ProcessCheckoutCommandHandlerTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<ICartService> _mockCartService;
        private readonly Mock<ILogger<ProcessCheckoutCommandHandler>> _mockLogger;
        private readonly Mock<ICacheService> _mockCacheService;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly Mock<IHttpContextAccessor> _mockHttpContextAccessor;
        private readonly Mock<IEmailService> _mockEmailService;
        private readonly ProcessCheckoutCommandHandler _handler;

        public ProcessCheckoutCommandHandlerTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockCartService = new Mock<ICartService>();
            _mockLogger = new Mock<ILogger<ProcessCheckoutCommandHandler>>();
            _mockCacheService = new Mock<ICacheService>();
            _mockUserManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(),
                null, null, null, null, null, null, null, null);
            _mockHttpContextAccessor = new Mock<IHttpContextAccessor>();
            _mockEmailService = new Mock<IEmailService>();
            _handler = new ProcessCheckoutCommandHandler(
                _mockLogger.Object,
                _mockMapper.Object,
                _mockUnitOfWork.Object,
                _mockCartService.Object,
                _mockCacheService.Object,
                _mockUserManager.Object,
                _mockHttpContextAccessor.Object,
                _mockEmailService.Object
            );
        }

        [Fact]
        public async Task Handle_CheckoutItemsEmpty_ReturnsNull()
        {
            // Arrange
            var request = new ProcessCheckoutCommand { CheckoutInfo = new CheckoutRequestDto() };

            _mockCacheService.Setup(c => c.Get<IEnumerable<CashCartDto>>("cashCartDtos")).Returns((IEnumerable<CashCartDto>)null);


            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_UserNotFound_ReturnsNull()
        {
            // Arrange
            var request = new ProcessCheckoutCommand { CheckoutInfo = new CheckoutRequestDto() };

            _mockCacheService.Setup(c => c.Get<IEnumerable<CashCartDto>>("cashCartDtos"))
                .Returns(new List<CashCartDto> { new CashCartDto() });
            //make the calim not null 
            _mockHttpContextAccessor.Setup(h => h.HttpContext.User.Claims).Returns(new List<Claim> {
            new Claim(ClaimTypes.Email, "Email@exmple.com") });
            _mockUserManager.Setup(u => u.FindByEmailAsync(It.IsAny<string>())).ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Null(result);
        }

        [Fact]
        public async Task Handle_CreateBookingAndPayment_ReturnsBookingConfirmationDto()
        {
            // Arrange
            var request = new ProcessCheckoutCommand { CheckoutInfo = new CheckoutRequestDto { CardNumber = "1001123412341234" } };

            var cashCartItems = new List<CashCartDto>
        {
            new CashCartDto { RoomId = 1, TotalPrice = 100, CheckInDate = DateTime.Today, CheckOutDate = DateTime.Today.AddDays(1) }
        };

            _mockCacheService.Setup(c => c.Get<IEnumerable<CashCartDto>>("cashCartDtos")).Returns(cashCartItems);

            _mockHttpContextAccessor.Setup(h => h.HttpContext.User.Claims).Returns(new List<Claim> {
            new Claim(ClaimTypes.Email, "Email@exmple.com") });

            var user = new User { Id = "1", Email = "Email@exmple.com" };

            _mockUserManager.Setup(u => u.FindByEmailAsync(user.Email)).ReturnsAsync(user);

            var room = new Room { HotelID = 1 };
            var hotel = new Hotel { Name = "HotelName", Address = "Address", HotelID = 1 };
            var booking = new Booking { BookingID = 1, UserID = user.Id, CheckInDate = DateTime.Today, CheckOutDate = DateTime.Today.AddDays(1) };
            var bookingRoom = new BookingRoom { BookingID = booking.BookingID, RoomID = room.RoomID };
            var payment = new Payment { PaymentID = 1, BookingID = booking.BookingID, PaymentDate = DateTime.Today };

            _mockUnitOfWork.Setup(u => u.Rooms.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(room);
            _mockUnitOfWork.Setup(u => u.Hotels.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(hotel);
            _mockUnitOfWork.Setup(u => u.Bookings.CreateAsync(It.IsAny<Booking>())).ReturnsAsync(booking);
            _mockUnitOfWork.Setup(u => u.BookingRooms.CreateAsync(It.IsAny<BookingRoom>())).ReturnsAsync(bookingRoom);
            _mockUnitOfWork.Setup(u => u.Payments.CreateAsync(It.IsAny<Payment>())).ReturnsAsync(payment);
            _mockUnitOfWork.Setup(u => u.BeginTransactionAsync()).ReturnsAsync(new Mock<IDbContextTransaction>().Object);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("HotelName", result.HotelName);
            Assert.Equal("Address", result.HotelAddress);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.AtLeastOnce);
            _mockEmailService.Verify(e => e.SendEmailAsync(user.Email, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Handle_CheckoutThrowsException_RollsBackTransactionAndSendsFailureEmail()
        {
            // Arrange
            var request = new ProcessCheckoutCommand { CheckoutInfo = new CheckoutRequestDto { CardNumber = "1001123412341234" } };

            var cashCartItems = new List<CashCartDto>
        {
            new CashCartDto { RoomId = 1, TotalPrice = 100, CheckInDate = DateTime.Today, CheckOutDate = DateTime.Today.AddDays(1) }
        };

            _mockCacheService.Setup(c => c.Get<IEnumerable<CashCartDto>>("cashCartDtos")).Returns(cashCartItems);
            var user = new User { Id = Guid.NewGuid().ToString(), Email = "user@example.com" };
            _mockHttpContextAccessor.Setup(h => h.HttpContext.User.Claims)
                .Returns(new List<Claim> { new Claim(ClaimTypes.Email, user.Email) });

            _mockUserManager.Setup(u => u.FindByEmailAsync(user.Email)).ReturnsAsync(user);

            var room = new Room { HotelID = 1 };
            var hotel = new Hotel { Name = "Test Hotel", Address = "Test Address", HotelID = 1 };
            var booking = new Booking { BookingID = 1, UserID = user.Id, CheckInDate = DateTime.Today, CheckOutDate = DateTime.Today.AddDays(1) };
            var bookingRoom = new BookingRoom { BookingID = booking.BookingID, RoomID = room.RoomID };
            var payment = new Payment { PaymentID = 1, BookingID = booking.BookingID, PaymentDate = DateTime.Today };

            _mockUnitOfWork.Setup(u => u.Rooms.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(room);
            _mockUnitOfWork.Setup(u => u.Hotels.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(hotel);
            _mockUnitOfWork.Setup(u => u.Bookings.CreateAsync(It.IsAny<Booking>())).ReturnsAsync(booking);
            _mockUnitOfWork.Setup(u => u.BookingRooms.CreateAsync(It.IsAny<BookingRoom>())).ReturnsAsync(bookingRoom);
            _mockUnitOfWork.Setup(u => u.Payments.CreateAsync(It.IsAny<Payment>())).ReturnsAsync(payment);
            _mockUnitOfWork.Setup(u => u.BeginTransactionAsync()).ReturnsAsync(new Mock<IDbContextTransaction>().Object);
            _mockUnitOfWork.Setup(u => u.CompleteAsync()).ThrowsAsync(new Exception("Database error"));


            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));

            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.AtLeastOnce);
            _mockEmailService.Verify(e => e.SendEmailAsync(user.Email, It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}