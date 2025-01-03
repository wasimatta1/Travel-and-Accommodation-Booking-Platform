using Application.Mediator.Commands.HotelCommands;
using Application.Mediator.Handlers.HotelHandler;
using AutoMapper;
using Contracts.DTOs.Hotel;
using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;

namespace Travel_and_Accommodation_Booking.UnitTests.Application.Mediator.Handlers.HotelHandlers
{
    public class UpdateHotelCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<UpdateHotelCommandHandler>> _mockLogger;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly UpdateHotelCommandHandler _handler;

        public UpdateHotelCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<UpdateHotelCommandHandler>>();

            var mockUserStore = new Mock<IUserStore<User>>();
            _mockUserManager = new Mock<UserManager<User>>(mockUserStore.Object,
                null, null, null, null, null, null, null, null);

            _handler = new UpdateHotelCommandHandler(
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockLogger.Object,
                _mockUserManager.Object);
        }

        [Fact]
        public async Task Handle_InvalidHotelId_ReturnsNull()
        {
            // Arrange
            var updateHotelDto = new UpdateHotelDto
            {
                HotelID = 99,
                Name = "HotelName",
                CityID = 1,
                OwnerID = "ownerId",
                AmenitiesName = new List<string> { "Amenity1", "Amenity2" }
            };

            var request = new UpdateHotelCommand { UpdateHotelDto = updateHotelDto };

            _mockUnitOfWork.Setup(u => u.Hotels.GetByIdAsync(request.UpdateHotelDto.HotelID))
                .ReturnsAsync((Hotel)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _mockUnitOfWork.Verify(u => u.Hotels.GetByIdAsync(updateHotelDto.HotelID), Times.Once);
            _mockUnitOfWork.Verify(u => u.Hotels.UpdateAsync(It.IsAny<Hotel>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Never);
        }
        [Fact]
        public async Task Handle_InvalidCityId_ReturnsNull()
        {
            // Arrange
            var updateHotelDto = new UpdateHotelDto
            {
                HotelID = 1,
                Name = "Updated Hotel",
                CityID = 99,
                OwnerID = "ownerId",
                AmenitiesName = new List<string> { "Amenity1", "Amenity2" }
            };

            var request = new UpdateHotelCommand { UpdateHotelDto = updateHotelDto };

            _mockUnitOfWork.Setup(u => u.Hotels.GetByIdAsync(request.UpdateHotelDto.HotelID))
                .ReturnsAsync(new Hotel());

            _mockUnitOfWork.Setup(u => u.Cities.GetByIdAsync(updateHotelDto.CityID))
                .ReturnsAsync((City)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _mockUnitOfWork.Verify(u => u.Cities.GetByIdAsync(updateHotelDto.CityID), Times.Once);
            _mockUnitOfWork.Verify(u => u.Hotels.UpdateAsync(It.IsAny<Hotel>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_InvalidOwnerId_ReturnsNull()
        {
            // Arrange
            var updateHotelDto = new UpdateHotelDto
            {
                HotelID = 1,
                Name = "Updated Hotel",
                CityID = 1,
                OwnerID = "invalidOwnerId",
                AmenitiesName = new List<string> { "Spa", "Gym" }
            };

            var request = new UpdateHotelCommand { UpdateHotelDto = updateHotelDto };

            var city = new City { CityID = updateHotelDto.CityID, Name = "CityName" };

            _mockUnitOfWork.Setup(u => u.Hotels.GetByIdAsync(request.UpdateHotelDto.HotelID))
                .ReturnsAsync(new Hotel());

            _mockUnitOfWork.Setup(u => u.Cities.GetByIdAsync(updateHotelDto.CityID))
                .ReturnsAsync(city);

            _mockUserManager.Setup(u => u.FindByIdAsync(updateHotelDto.OwnerID))
                .ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _mockUserManager.Verify(u => u.FindByIdAsync(updateHotelDto.OwnerID), Times.Once);
            _mockUnitOfWork.Verify(u => u.Hotels.UpdateAsync(It.IsAny<Hotel>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ValidHotelId_NoAmenitiesProvided_ClearsHotelAmenities()
        {
            // Arrange
            var hotelId = 1;
            var updateHotelDto = new UpdateHotelDto
            {
                HotelID = hotelId,
                Name = "UpdatedHotelName",
                CityID = 1,
                OwnerID = "ownerId",
                AmenitiesName = null
            };

            var hotel = new Hotel
            {
                HotelID = hotelId,
                Name = "OldHotelName",
                HotelAmenities = new List<HotelAmenity>
            {
                new HotelAmenity { AmenityID = 1 }
            }
            };

            var updatedHotelDto = new HotelDto
            {
                Name = "UpdatedHotelName",
                AmenitiesName = null
            };

            var request = new UpdateHotelCommand { UpdateHotelDto = updateHotelDto };

            _mockUnitOfWork.Setup(u => u.Hotels.GetByIdAsync(request.UpdateHotelDto.HotelID))
                .ReturnsAsync(hotel);
            _mockUnitOfWork.Setup(u => u.Cities.GetByIdAsync(updateHotelDto.CityID))
                .ReturnsAsync(new City());
            _mockUserManager.Setup(u => u.FindByIdAsync(updateHotelDto.OwnerID))
                .ReturnsAsync(new User());

            _mockMapper.Setup(m => m.Map(updateHotelDto, hotel));
            _mockUnitOfWork.Setup(u => u.Hotels.UpdateAsync(hotel)).ReturnsAsync(hotel);
            _mockUnitOfWork.Setup(u => u.HotelAmenities.DeleteRangeAsync(hotel.HotelAmenities));

            _mockMapper.Setup(m => m.Map<HotelDto>(hotel)).Returns(updatedHotelDto);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedHotelDto.Name, result.Name);
            Assert.Null(result.AmenitiesName);

            _mockUnitOfWork.Verify(u => u.HotelAmenities.DeleteRangeAsync(hotel.HotelAmenities), Times.Once);
            _mockUnitOfWork.Verify(u => u.HotelAmenities.AddRangeAsync(It.IsAny<IEnumerable<HotelAmenity>>()), Times.Never);
            _mockUnitOfWork.Verify(u => u.Hotels.UpdateAsync(hotel), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Exactly(2));
        }

        [Fact]
        public async Task Handle_ValidHotelId_UpdatesHotelAndReturnsDto()
        {
            // Arrange
            var hotelId = 1;
            var updateHotelDto = new UpdateHotelDto
            {
                HotelID = hotelId,
                Name = "UpdatedHotelName",
                CityID = 1,
                OwnerID = "ownerId",
                AmenitiesName = new List<string> { "Amenity1", "Amenity2" }
            };

            var hotel = new Hotel
            {
                HotelID = hotelId,
                Name = "OldHotelName",
                HotelAmenities = new List<HotelAmenity>
            {
                new HotelAmenity { AmenityID = 1 }
            }
            };

            var updatedHotelDto = new HotelDto
            {
                Name = "UpdatedHotelName",
                AmenitiesName = updateHotelDto.AmenitiesName
            };

            var request = new UpdateHotelCommand { UpdateHotelDto = updateHotelDto };


            _mockUnitOfWork.Setup(u => u.Hotels.GetByIdAsync(request.UpdateHotelDto.HotelID))
                .ReturnsAsync(hotel);
            _mockUnitOfWork.Setup(u => u.Cities.GetByIdAsync(updateHotelDto.CityID))
                .ReturnsAsync(new City());
            _mockUserManager.Setup(u => u.FindByIdAsync(updateHotelDto.OwnerID))
                .ReturnsAsync(new User());

            _mockMapper.Setup(m => m.Map(updateHotelDto, hotel));
            _mockUnitOfWork.Setup(u => u.Hotels.UpdateAsync(hotel)).ReturnsAsync(hotel);
            _mockUnitOfWork.Setup(u => u.HotelAmenities.DeleteRangeAsync(hotel.HotelAmenities));
            _mockUnitOfWork.Setup(u => u.Amenities.FindAsync(It.IsAny<Expression<Func<Amenity, bool>>>(), null))
                .ReturnsAsync(new Amenity { AmenitiesID = 1, Name = "Amenity1" });

            _mockMapper.Setup(m => m.Map<HotelDto>(hotel)).Returns(updatedHotelDto);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(updatedHotelDto.Name, result.Name);
            Assert.Equal(updatedHotelDto.AmenitiesName, result.AmenitiesName);

            _mockUnitOfWork.Verify(u => u.HotelAmenities.DeleteRangeAsync(hotel.HotelAmenities), Times.Once);
            _mockUnitOfWork.Verify(u => u.HotelAmenities.AddRangeAsync(It.IsAny<IEnumerable<HotelAmenity>>()), Times.Once);
            _mockUnitOfWork.Verify(u => u.Hotels.UpdateAsync(hotel), Times.Once);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Exactly(2));
        }
    }
}