using Application.Mediator.Commands.RoomCommands;
using Application.Mediator.Handlers.RoomHandler;
using AutoMapper;
using Contracts.DTOs.Room;
using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Travel_and_Accommodation_Booking.UnitTests.Application.Mediator.Handlers.RoomHandlers
{
    public class UpdateRoomCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<UpdateRoomCommandHnadler>> _mockLogger;
        private readonly UpdateRoomCommandHnadler _handler;

        public UpdateRoomCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<UpdateRoomCommandHnadler>>();
            _handler = new UpdateRoomCommandHnadler(
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockLogger.Object);
        }
        [Fact]
        public async Task Handle_InvalidRoomId_ReturnsNull()
        {
            // Arrange
            var updateRoomDto = new UpdateRoomDto
            {
                RoomID = 99,
                RoomNumber = "1",
                ImagesUrl = new List<string> { "Url1", "Url2" }
            };

            var request = new UpdateRoomCommand { UpdateRoomDto = updateRoomDto };

            _mockUnitOfWork.Setup(u => u.Rooms.FindAsync(c => c.RoomID == request.UpdateRoomDto.RoomID, It.IsAny<string[]>()))
                .ReturnsAsync((Room)null);


            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _mockUnitOfWork.Verify(u => u.Rooms.FindAsync(It.IsAny<Expression<Func<Room, bool>>>(), It.IsAny<string[]>())
            , Times.Once);
            _mockUnitOfWork.Verify(u => u.Rooms.UpdateAsync(It.IsAny<Room>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ValidRoomId_NoImagesProvided_ClearsRoomImages()
        {
            // Arrange
            var roomId = 1;
            var updateRoomDto = new UpdateRoomDto
            {
                RoomID = roomId,
                RoomNumber = "1",
                ImagesUrl = null
            };

            var room = new Room
            {
                RoomID = roomId,
                RoomNumber = "1",
                RoomImages = new List<RoomImage>
            {
                new RoomImage { ImageUrl = "oldUrl1" }
            }
            };

            var updatedRoomDto = new RoomDto
            {
                RoomNumber = "1",
                ImagesUrl = null
            };

            var request = new UpdateRoomCommand { UpdateRoomDto = updateRoomDto };

            _mockUnitOfWork.Setup(u => u.Rooms.FindAsync(c => c.RoomID == request.UpdateRoomDto.RoomID, It.IsAny<string[]>()))
                .ReturnsAsync(room);

            _mockMapper.Setup(m => m.Map(updateRoomDto, room));
            _mockUnitOfWork.Setup(u => u.Rooms.UpdateAsync(room)).ReturnsAsync(room);
            _mockUnitOfWork.Setup(u => u.RoomImages.DeleteRangeAsync(room.RoomImages));

            _mockMapper.Setup(m => m.Map<RoomDto>(room)).Returns(updatedRoomDto);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedRoomDto.RoomNumber, result.RoomNumber);
            Assert.Null(result.ImagesUrl);

            _mockUnitOfWork.Verify(u => u.RoomImages.DeleteRangeAsync(room.RoomImages), Times.Once);
            _mockUnitOfWork.Verify(u => u.RoomImages.AddRangeAsync(It.IsAny<IEnumerable<RoomImage>>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.Rooms.UpdateAsync(room), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Exactly(2));
        }

        [Fact]
        public async Task Handle_ValidRoomId_UpdatesRoomAndReturnsDto()
        {
            // Arrange
            var roomId = 1;
            var updateRoomDto = new UpdateRoomDto
            {
                RoomID = roomId,
                RoomNumber = "1",
                ImagesUrl = new List<string> { "NewUrl1", "NewUrl2" }
            };

            var room = new Room
            {
                RoomID = roomId,
                RoomNumber = "1",
                RoomImages = new List<RoomImage>
            {
                new RoomImage { ImageUrl = "oldUel" }
            }
            };

            var updatedRoomDto = new RoomDto
            {
                RoomNumber = "2",
                ImagesUrl = updateRoomDto.ImagesUrl
            };

            var request = new UpdateRoomCommand { UpdateRoomDto = updateRoomDto };

            _mockUnitOfWork.Setup(u => u.Rooms.FindAsync(It.IsAny<Expression<Func<Room, bool>>>(), It.IsAny<string[]>()))
                .ReturnsAsync(room);

            _mockMapper.Setup(m => m.Map(updateRoomDto, room));
            _mockUnitOfWork.Setup(u => u.Rooms.UpdateAsync(room)).ReturnsAsync(room);
            _mockUnitOfWork.Setup(u => u.RoomImages.DeleteRangeAsync(room.RoomImages));

            _mockMapper.Setup(m => m.Map<RoomDto>(room)).Returns(updatedRoomDto);


            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedRoomDto.RoomNumber, result.RoomNumber);
            Assert.Equal(updatedRoomDto.ImagesUrl, result.ImagesUrl);

            _mockUnitOfWork.Verify(u => u.RoomImages.DeleteRangeAsync(room.RoomImages), Times.Once);
            _mockUnitOfWork.Verify(u => u.RoomImages.AddRangeAsync(It.IsAny<IEnumerable<RoomImage>>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.Rooms.UpdateAsync(room), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Exactly(2));
        }
    }
}