using Application.Mediator.Commands.RoomCommands;
using Application.Mediator.Handlers.RoomHandler;
using AutoMapper;
using Contracts.DTOs.Room;
using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Travel_and_Accommodation_Booking.UnitTests.Application.Mediator.Handlers.RoomHandlers
{
    public class CreateRoomCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<CreateRoomCommandHandler>> _mockLogger;
        private readonly CreateRoomCommandHandler _handler;

        public CreateRoomCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<CreateRoomCommandHandler>>();

            _handler = new CreateRoomCommandHandler(
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockLogger.Object);
        }
        [Fact]
        public async Task Handle_InvalidHotel_ReturnsNegativeOne()
        {
            // Arrange
            var command = new CreateRoomCommand
            {
                CreateRoomDto = new CreateRoomDto
                {
                    HotelID = 99
                }
            };

            _mockUnitOfWork.Setup(u => u.Hotels.GetByIdAsync(command.CreateRoomDto.HotelID)).ReturnsAsync((Hotel)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(-1, result);
            _mockUnitOfWork.Verify(u => u.Hotels.GetByIdAsync(command.CreateRoomDto.HotelID), Times.Once);
        }

        [Fact]
        public async Task Handle_EmptyImageUrlList_HandlesSuccessfully()
        {
            // Arrange
            var command = new CreateRoomCommand
            {
                CreateRoomDto = new CreateRoomDto
                {
                    RoomNumber = "1",
                    HotelID = 1,
                    ImagesUrl = new List<string>()
                }
            };

            var hotel = new Hotel { HotelID = command.CreateRoomDto.HotelID, Name = "HotelName" };
            var room = new Room { RoomID = 1, RoomNumber = command.CreateRoomDto.RoomNumber, HotelID = command.CreateRoomDto.HotelID };

            _mockUnitOfWork.Setup(u => u.Hotels.GetByIdAsync(command.CreateRoomDto.HotelID)).ReturnsAsync(hotel);
            _mockMapper.Setup(m => m.Map<Room>(command.CreateRoomDto)).Returns(room);
            _mockUnitOfWork.Setup(u => u.Rooms.CreateAsync(room)).ReturnsAsync(room);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(room.RoomID, result);
            _mockMapper.Verify(m => m.Map<Room>(command.CreateRoomDto), Times.Once);
            _mockUnitOfWork.Verify(u => u.Hotels.GetByIdAsync(command.CreateRoomDto.HotelID), Times.Once);
            _mockUnitOfWork.Verify(u => u.Rooms.CreateAsync(room), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_ValidRoom_ReturnsRoomId()
        {
            // Arrange
            var command = new CreateRoomCommand
            {
                CreateRoomDto = new CreateRoomDto
                {
                    RoomNumber = "1",
                    HotelID = 1,
                    ImagesUrl = new List<string> { "url1", "url2" }
                }
            };

            var hotel = new Hotel { HotelID = command.CreateRoomDto.HotelID, Name = "Test Hotel" };
            var room = new Room { RoomID = 1, RoomNumber = command.CreateRoomDto.RoomNumber, HotelID = command.CreateRoomDto.HotelID };
            var roomImages = new List<RoomImage>
        {
            new RoomImage { RoomID = room.RoomID, ImageUrl = "url1" },
            new RoomImage { RoomID = room.RoomID, ImageUrl = "url2" }
        };

            _mockUnitOfWork.Setup(u => u.Hotels.GetByIdAsync(command.CreateRoomDto.HotelID)).ReturnsAsync(hotel);
            _mockMapper.Setup(m => m.Map<Room>(command.CreateRoomDto)).Returns(room);
            _mockUnitOfWork.Setup(u => u.Rooms.CreateAsync(room)).ReturnsAsync(room);
            _mockUnitOfWork.Setup(u => u.RoomImages.AddRangeAsync(It.IsAny<List<RoomImage>>())).ReturnsAsync(roomImages);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(room.RoomID, result);
            _mockMapper.Verify(m => m.Map<Room>(command.CreateRoomDto), Times.Once);
            _mockUnitOfWork.Verify(u => u.Hotels.GetByIdAsync(command.CreateRoomDto.HotelID), Times.Once);
            _mockUnitOfWork.Verify(u => u.Rooms.CreateAsync(room), Times.Once);
            _mockUnitOfWork.Verify(u => u.RoomImages.AddRangeAsync(It.IsAny<List<RoomImage>>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Exactly(2));
        }
    }
}