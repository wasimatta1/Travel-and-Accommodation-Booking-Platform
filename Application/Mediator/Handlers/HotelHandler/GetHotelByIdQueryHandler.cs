using Application.Mediator.Queries.HotelQueries;
using AutoMapper;
using Contracts.DTOs.Hotel;
using Contracts.Interfaces.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.HotelHandler
{
    public class GetHotelByIdQueryHandler : IRequestHandler<GetHotelByIdQuery, HotelDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetHotelByIdQueryHandler> _logger;

        public GetHotelByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetHotelByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<HotelDto> Handle(GetHotelByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling GetHotelByIdQuery for Hotel ID: {request.HotelID}");

            // include owner table and city table
            var includes = new[]
            {
                "Owner",
                "City",
                "Rooms",
                "Amenities"
            };

            var hotel = await _unitOfWork.Hotels.FindAsync(c => c.HotelID == request.HotelID, includes);

            if (hotel == null)
            {
                _logger.LogWarning($"Hotel with ID: {request.HotelID} was not found");
                return null;
            }

            var hotelDto = _mapper.Map<HotelDto>(hotel);


            return hotelDto;
        }
    }
}