using Application.Mediator.Handlers.RoomHandler;
using Application.Mediator.Queries.RoomQueries;
using AutoMapper;
using Contracts.DTOs.Room;
using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Travel_and_Accommodation_Booking.UnitTests.Application.Mediator.Handlers.RoomHandlers
{
    public class GetRoomByIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<GetRoomByIdQueryHandler>> _mockLogger;
        private readonly GetRoomByIdQueryHandler _handler;

        public GetRoomByIdQueryHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<GetRoomByIdQueryHandler>>();
            _handler = new GetRoomByIdQueryHandler(
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_InvalidRoomId_ReturnsNull()
        {
            // Arrange
            var request = new GetRoomByIdQuery { RoomID = 99 };

            _mockUnitOfWork.Setup(u => u.Rooms.FindAsync(r => r.RoomID == request.RoomID, It.IsAny<string[]>()))
                .ReturnsAsync((Room)null);


            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _mockUnitOfWork.Verify(u => u.Rooms.FindAsync(It.IsAny<Expression<Func<Room, bool>>>(), It.IsAny<string[]>()), Times.Once);
            _mockMapper.Verify(m => m.Map<RoomDto>(It.IsAny<Room>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ValidRoomId_ReturnsRoomDto()
        {
            // Arrange
            var roomId = 1;
            var room = new Room
            {
                RoomID = roomId,
                RoomNumber = "1",
                Hotel = new Hotel { HotelID = 1, Name = "HotelName" },
                RoomImages = new List<RoomImage>
            {
                new RoomImage { ImageUrl = "Url1" },
                new RoomImage { ImageUrl = "Url2" }
            }
            };

            var roomDto = new RoomDto
            {
                RoomNumber = "1",
                HotelName = "HotelName",
                ImagesUrl = new List<string> { "Url1", "Url2" }
            };

            var request = new GetRoomByIdQuery { RoomID = roomId };

            _mockUnitOfWork.Setup(u => u.Rooms.FindAsync(r => r.RoomID == request.RoomID, It.IsAny<string[]>()))
                .ReturnsAsync(room);

            _mockMapper.Setup(m => m.Map<RoomDto>(room)).Returns(roomDto);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(roomDto.RoomNumber, result.RoomNumber);
            Assert.Equal(roomDto.HotelName, result.HotelName);
            Assert.Equal(roomDto.ImagesUrl, result.ImagesUrl);

            _mockUnitOfWork.Verify(u => u.Rooms.FindAsync(It.IsAny<Expression<Func<Room, bool>>>(), It.IsAny<string[]>()), Times.Once);
            _mockMapper.Verify(m => m.Map<RoomDto>(room), Times.Once);
        }

    }
}