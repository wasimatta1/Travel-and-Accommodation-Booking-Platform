using Application.Mediator.Handlers.HotelHandler;
using Application.Mediator.Queries.HotelQueries;
using AutoMapper;
using Contracts.DTOs.Hotel;
using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using Microsoft.Extensions.Logging;
using Moq;
using System.Linq.Expressions;
namespace Travel_and_Accommodation_Booking.UnitTests.Application.Mediator.Handlers.HotelHandlers
{
    public class GetHotelByIdQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ILogger<GetHotelByIdQueryHandler>> _mockLogger;
        private readonly GetHotelByIdQueryHandler _handler;

        public GetHotelByIdQueryHandlerTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockMapper = new Mock<IMapper>();
            _mockLogger = new Mock<ILogger<GetHotelByIdQueryHandler>>();
            _handler = new GetHotelByIdQueryHandler(_mockUnitOfWork.Object, _mockMapper.Object, _mockLogger.Object);
        }

        [Fact]
        public async Task Handle_HotelDoesNotExist_ReturnsNull()
        {
            // Arrange
            var hotelId = 1;

            _mockUnitOfWork
                .Setup(u => u.Hotels.FindAsync(It.IsAny<Expression<Func<Hotel, bool>>>(), It.IsAny<string[]>()))
                .ReturnsAsync((Hotel)null);

            var query = new GetHotelByIdQuery { HotelID = hotelId };

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.Null(result);
            _mockUnitOfWork.Verify(u => u.Hotels.FindAsync(It.IsAny<Expression<Func<Hotel, bool>>>(), It.IsAny<string[]>()), Times.Once);
            _mockMapper.Verify(m => m.Map<HotelDto>(It.IsAny<Hotel>()), Times.Never);
        }
        [Fact]
        public async Task Handle_HotelExists_ReturnsHotelDto()
        {
            // Arrange
            var hotelId = 1;
            var hotel = new Hotel { HotelID = hotelId, Name = "HotelName" };
            var hotelDto = new HotelDto { Name = "HotelName" };

            var request = new GetHotelByIdQuery { HotelID = hotelId };

            _mockUnitOfWork
                .Setup(u => u.Hotels.FindAsync(c => c.HotelID == request.HotelID, It.IsAny<string[]>()))
                .ReturnsAsync(hotel);

            _mockMapper.Setup(m => m.Map<HotelDto>(hotel)).Returns(hotelDto);


            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            _mockUnitOfWork.Verify(u => u.Hotels.FindAsync(It.IsAny<Expression<Func<Hotel, bool>>>(), It.IsAny<string[]>()), Times.Once);
            _mockMapper.Verify(m => m.Map<HotelDto>(hotel), Times.Once);
        }

    }
}