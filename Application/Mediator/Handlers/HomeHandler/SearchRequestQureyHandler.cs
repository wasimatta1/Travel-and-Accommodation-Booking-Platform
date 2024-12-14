using Application.Mediator.Queries.HomeQueries;
using AutoMapper;
using Contracts.DTOs.Home;
using Contracts.Interfaces.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.HomeHandler
{
    public class SearchRequestQureyHandler : IRequestHandler<SearchRequestQurey, IEnumerable<SearchResultDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<SearchRequestQureyHandler> _logger;

        public SearchRequestQureyHandler(IUnitOfWork unitOfWork, ILogger<SearchRequestQureyHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<IEnumerable<SearchResultDto>> Handle(SearchRequestQurey request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("SearchRequestQureyHandler.Handle called");

            var hotels = await _unitOfWork.Hotels.SearchHotels(request.Query, request.StarRating, request.PageNumber, request.PageSize,
                request.Amenities, request.CheckInDate, request.CheckOutDate, request.Adults, request.Children, request.Rooms,
                request.PriceMin, request.PriceMax, request.RoomType);

            _logger.LogInformation("SearchRequestQureyHandler.Handle finished");
            return _mapper.Map<IEnumerable<SearchResultDto>>(hotels);
        }

    }
}
