using Application.Mediator.Queries.HomeQueries;
using AutoMapper;
using Contracts.DTOs.Home;
using Contracts.Interfaces.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.HomeHandler
{
    public class GetTreandingDestinationsQueryHandler : IRequestHandler<GetTrendingDestinationsQuery,
        IEnumerable<TrendingDestinationDto>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetTreandingDestinationsQueryHandler> _logger;

        public GetTreandingDestinationsQueryHandler(IUnitOfWork unitOfWork,
            ILogger<GetTreandingDestinationsQueryHandler> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<IEnumerable<TrendingDestinationDto>> Handle(GetTrendingDestinationsQuery request,
            CancellationToken cancellationToken)
        {
            _logger.LogInformation("GetTreandingDestinationsQueryHandler.Handle called");

            var cities = await _unitOfWork.Cities.GetTrendingDestinationsCitiesAsync(request.Take);

            _logger.LogInformation("GetTreandingDestinationsQueryHandler.Handle finished");
            return _mapper.Map<IEnumerable<TrendingDestinationDto>>(cities);
        }
    }
}
