using Application.Mediator.Handlers.HotelPageHandler;
using Application.Mediator.Queries.HotelPageQueries;
using AutoMapper;
using Contracts.DTOs.HotelPage;
using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;

namespace Travel_and_Accommodation_Booking.UnitTests.Application.Mediator.Handlers.HotelPageHandlers
{
    public class GetHotelPageQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<GetHotelPageQueryHandler>> _mockLogger;
        private readonly GetHotelPageQueryHandler _handler;

        public GetHotelPageQueryHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<GetHotelPageQueryHandler>>();
            _handler = new GetHotelPageQueryHandler(
                _mockUnitOfWork.Object,
                _mockLogger.Object,
                _mockMapper.Object);
        }

        [Fact]
        public async Task Handle_ValidRequest_ReturnsHotelPageDto()
        {
            var request = new GetHotelPageQuery
            {
                HotelId = 1,
                CheckInDate = DateTime.Now,
                CheckOutDate = DateTime.Now.AddDays(2),
                Adults = 2,
                Children = 1
            };

            var hotel = new Hotel
            {
                HotelID = request.HotelId,
                Name = "HotelName",
                Address = "1Address",
            };

            var hotelPageDto = new HotelPageDto
            {
                HotelName = hotel.Name,
            };

            _mockUnitOfWork.Setup(u => u.Hotels.GetHotelPageIdAsync(request.HotelId, request.CheckInDate, request.CheckOutDate,
                    request.Adults, request.Children, request.PriceMin, request.PriceMax, request.RoomType))
                .ReturnsAsync(hotel);

            _mockMapper.Setup(m => m.Map<HotelPageDto>(hotel))
                .Returns(hotelPageDto);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(hotel.Name, result.HotelName);


            _mockUnitOfWork.Verify(u => u.Hotels.GetHotelPageIdAsync(
                request.HotelId, request.CheckInDate, request.CheckOutDate,
                request.Adults, request.Children, request.PriceMin, request.PriceMax, request.RoomType), Times.Once);
        }

        [Fact]
        public async Task Handle_HotelNotFound_ReturnsNull()
        {
            // Arrange
            var request = new GetHotelPageQuery
            {
                HotelId = 99,
                CheckInDate = DateTime.Now,
                CheckOutDate = DateTime.Now.AddDays(2),
                Adults = 2,
                Children = 1
            };

            _mockUnitOfWork.Setup(u => u.Hotels.GetHotelPageIdAsync(request.HotelId, request.CheckInDate, request.CheckOutDate,
                    request.Adults, request.Children, request.PriceMin, request.PriceMax, request.RoomType))
                .ReturnsAsync((Hotel)null);

            _mockMapper.Setup(m => m.Map<HotelPageDto>(It.IsAny<Hotel>())).Returns((HotelPageDto)null);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Null(result);

            _mockUnitOfWork.Verify(u => u.Hotels.GetHotelPageIdAsync(
                request.HotelId, request.CheckInDate, request.CheckOutDate,
                request.Adults, request.Children, request.PriceMin, request.PriceMax, request.RoomType), Times.Once);
        }
    }
}