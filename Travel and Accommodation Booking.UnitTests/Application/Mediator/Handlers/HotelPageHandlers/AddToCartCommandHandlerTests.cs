using Application.Mediator.Commands.HotelPageCommands;
using Application.Mediator.Handlers.HotelPageHandler;
using Contracts.DTOs.HotelPage;
using Contracts.Interfaces;
using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Travel_and_Accommodation_Booking.UnitTests.Application.Mediator.Handlers.HotelPageHandlers
{
    public class AddToCartCommandHandlerTests
    {
        private readonly Mock<ICartService> _mockCartService;
        private readonly Mock<ILogger<AddToCartCommandHandler>> _mockLogger;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly AddToCartCommandHandler _handler;

        public AddToCartCommandHandlerTests()
        {
            _mockCartService = new Mock<ICartService>();
            _mockLogger = new Mock<ILogger<AddToCartCommandHandler>>();
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _handler = new AddToCartCommandHandler(_mockCartService.Object, _mockLogger.Object, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task Handle_ItemAlreadyInCart_ReturnsFalse()
        {
            // Arrange
            var existingItem = new AddRoomToCartDto { RoomId = 1 };
            var cartItems = new List<AddRoomToCartDto> { existingItem };
            var request = new AddToCartCommand
            {
                CartItem = new AddRoomToCartDto { RoomId = existingItem.RoomId }
            };

            _mockCartService.Setup(c => c.GetCartItems()).Returns(cartItems);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Handle_ItemFromDifferentHotel_ReturnsFalse()
        {
            // Arrange
            var cartItems = new List<AddRoomToCartDto>
        {
            new AddRoomToCartDto { RoomId = 1},
        };
            var existingRoom = new Room { RoomID = 1, HotelID = 1 };
            var requestedRoom = new Room { RoomID = 2, HotelID = 2 };

            var request = new AddToCartCommand
            {
                CartItem = new AddRoomToCartDto { RoomId = requestedRoom.RoomID }
            };

            _mockCartService.Setup(c => c.GetCartItems()).Returns(cartItems);
            _mockUnitOfWork.Setup(u => u.Rooms.GetByIdAsync(requestedRoom.RoomID)).ReturnsAsync(requestedRoom);
            _mockUnitOfWork.Setup(u => u.Rooms.GetByIdAsync(cartItems.First().RoomId)).ReturnsAsync(existingRoom);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public async Task Handle_FirstItemAdd_ReturnsTrue()
        {
            // Arrange
            var cartItems = new List<AddRoomToCartDto>();
            var requestedRoom = new Room { RoomID = 1, HotelID = 1 };
            var command = new AddToCartCommand
            {
                CartItem = new AddRoomToCartDto { RoomId = requestedRoom.RoomID }
            };

            _mockCartService.Setup(c => c.GetCartItems()).Returns(cartItems);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mockCartService.Verify(c => c.AddToCart(It.Is<AddRoomToCartDto>(ci => ci.RoomId == requestedRoom.RoomID)), Times.Once);
            _mockUnitOfWork.Verify(u => u.Rooms.GetByIdAsync(requestedRoom.RoomID), Times.Never);
        }
        [Fact]
        public async Task Handle_NewItemWithSameHotel_ReturnsTrue()
        {
            // Arrange
            var cartItems = new List<AddRoomToCartDto>(
                new List<AddRoomToCartDto>
                {
                new AddRoomToCartDto { RoomId = 1 }
                }
                );
            var existingRoom = new Room { RoomID = 1, HotelID = 1 };
            var requestedRoom = new Room { RoomID = 2, HotelID = 1 };
            var command = new AddToCartCommand
            {
                CartItem = new AddRoomToCartDto { RoomId = requestedRoom.RoomID }
            };

            _mockCartService.Setup(c => c.GetCartItems()).Returns(cartItems);
            _mockUnitOfWork.Setup(u => u.Rooms.GetByIdAsync(requestedRoom.RoomID)).ReturnsAsync(requestedRoom);
            _mockUnitOfWork.Setup(u => u.Rooms.GetByIdAsync(cartItems.First().RoomId)).ReturnsAsync(existingRoom);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result);
            _mockCartService.Verify(c => c.AddToCart(It.Is<AddRoomToCartDto>(ci => ci.RoomId == requestedRoom.RoomID)), Times.Once);
            _mockUnitOfWork.Verify(u => u.Rooms.GetByIdAsync(requestedRoom.RoomID), Times.Once);
            _mockUnitOfWork.Verify(u => u.Rooms.GetByIdAsync(existingRoom.RoomID), Times.Once);
        }
    }
}