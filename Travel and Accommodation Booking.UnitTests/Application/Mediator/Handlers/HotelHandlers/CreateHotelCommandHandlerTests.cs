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
    public class CreateHotelCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<CreateHotelCommandHandler>> _mockLogger;
        private readonly Mock<UserManager<User>> _mockUserManager;
        private readonly CreateHotelCommandHandler _handler;

        public CreateHotelCommandHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<CreateHotelCommandHandler>>();
            _mockUserManager = new Mock<UserManager<User>>(
                new Mock<IUserStore<User>>().Object, null, null, null, null, null, null, null, null);

            _handler = new CreateHotelCommandHandler(
                _mockUnitOfWork.Object,
                _mockMapper.Object,
                _mockLogger.Object,
                _mockUserManager.Object);
        }

        [Fact]
        public async Task Handle_InvalidCityId_ReturnsMinusOne()
        {
            // Arrange
            var createHotelDto = new CreateHotelDto
            {
                CityID = 99,
                Name = "HotelName",
                OwnerID = "OwnerID",
                AmenitiesName = new List<string>()
            };
            var command = new CreateHotelCommand { CreateHotelDto = createHotelDto };

            _mockUnitOfWork.Setup(u => u.Cities.GetByIdAsync(createHotelDto.CityID)).ReturnsAsync((City)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(-1, result);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_InvalidOwnerId_ReturnsMinusOne()
        {
            // Arrange
            var createHotelDto = new CreateHotelDto
            {
                CityID = 1,
                Name = "HotelName",
                OwnerID = "invalidOwnerID",
                AmenitiesName = new List<string>()
            };
            var command = new CreateHotelCommand { CreateHotelDto = createHotelDto };

            _mockUnitOfWork.Setup(u => u.Cities.GetByIdAsync(createHotelDto.CityID)).ReturnsAsync(new City());
            _mockUserManager.Setup(um => um.FindByIdAsync(createHotelDto.OwnerID)).ReturnsAsync((User)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(-1, result);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Never);
        }

        [Fact]
        public async Task Handle_ValidRequest_CreatesHotelSuccessfully()
        {
            // Arrange
            var createHotelDto = new CreateHotelDto
            {
                CityID = 1,
                Name = "HotelName",
                OwnerID = "OwnerID",
                AmenitiesName = new List<string> { "Amenity1", "Amenity2" }
            };
            var command = new CreateHotelCommand { CreateHotelDto = createHotelDto };

            var city = new City { CityID = 1, Name = "CityName" };
            var user = new User { Id = "OwnerID", UserName = "UserName" };
            var hotel = new Hotel { HotelID = 1, Name = "HotelName", CityID = 1 };

            _mockUnitOfWork.Setup(u => u.Cities.GetByIdAsync(createHotelDto.CityID)).ReturnsAsync(city);
            _mockUserManager.Setup(um => um.FindByIdAsync(createHotelDto.OwnerID)).ReturnsAsync(user);
            _mockMapper.Setup(m => m.Map<Hotel>(createHotelDto)).Returns(hotel);
            _mockUnitOfWork.Setup(u => u.Hotels.CreateAsync(hotel)).ReturnsAsync(hotel);
            string amenityName = "Amenity";
            _mockUnitOfWork.Setup(u => u.Amenities.FindAsync(It.IsAny<Expression<Func<Amenity, bool>>>(), null))
                .ReturnsAsync(new Amenity { AmenitiesID = 1, Name = amenityName });


            _mockUnitOfWork.Setup(u => u.HotelAmenities.AddRangeAsync(It.IsAny<IEnumerable<HotelAmenity>>()))
                .ReturnsAsync(new List<HotelAmenity>());

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.Equal(hotel.HotelID, result);
            _mockUnitOfWork.Verify(u => u.CompleteAsync(), Times.Exactly(2));
        }
    }
}