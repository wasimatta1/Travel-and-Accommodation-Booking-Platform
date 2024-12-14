using Application.Mediator.Queries.HotelPageQueries;
using AutoMapper;
using Contracts.DTOs.HotelPage;
using Contracts.Interfaces.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.HotelPageHandler
{
    public class GetHotelPageQueryHandler : IRequestHandler<GetHotelPageQuery, HotelPageDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetHotelPageQueryHandler> _logger;

        public GetHotelPageQueryHandler(IUnitOfWork unitOfWork, ILogger<GetHotelPageQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public async Task<HotelPageDto> Handle(GetHotelPageQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetHotelPageQueryHandler.Handle called");

            var hotel = await _unitOfWork.Hotels.GetHotelPageIdAsync(request.HotelId, request.CheckInDate, request.CheckOutDate,
                request.Adults, request.Children, request.PriceMin, request.PriceMax, request.RoomType);

            _logger.LogInformation("GetHotelPageQueryHandler.Handle returned");
            return _mapper.Map<HotelPageDto>(hotel);

        }
    }
}
