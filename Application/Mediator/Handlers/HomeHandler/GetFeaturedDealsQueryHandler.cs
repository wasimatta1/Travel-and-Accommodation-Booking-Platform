using Application.Mediator.Queries.HomeQueries;
using AutoMapper;
using Contracts.DTOs.Home;
using Contracts.Interfaces.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.HomeHandler
{
    public class GetFeaturedDealsQueryHandler : IRequestHandler<GetFeaturedDealsQuery, IEnumerable<FeaturedDealResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetFeaturedDealsQueryHandler> _logger;

        public GetFeaturedDealsQueryHandler(IUnitOfWork unitOfWork, ILogger<GetFeaturedDealsQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<IEnumerable<FeaturedDealResponse>> Handle(GetFeaturedDealsQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetFeaturedDealsQueryHandler.Handle called");

            var rooms = await _unitOfWork.Rooms.GetFeaturedRoomsAsync(request.Take);

            _logger.LogInformation("GetFeaturedDealsQueryHandler.Handle finished");
            return _mapper.Map<IEnumerable<FeaturedDealResponse>>(rooms);
        }
    }
}
