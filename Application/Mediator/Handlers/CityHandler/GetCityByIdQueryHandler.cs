using Application.Mediator.Queries.CityQueries;
using AutoMapper;
using Contracts.DTOs.City;
using Contracts.Interfaces.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.CityHandler
{
    public class GetCityByIdQueryHandler : IRequestHandler<GetCityByIdQuery, CityDto>
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCityByIdQueryHandler> _logger;

        public GetCityByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetCityByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CityDto> Handle(GetCityByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling GetCityByIdQuery for City ID: {CityID}", request.CityID);

            var city = await _unitOfWork.Cities.GetByIdAsync(request.CityID);

            if (city == null)
            {
                _logger.LogWarning("City with ID: {CityID} was not found", request.CityID);
                return null;
            }

            var cityDto = _mapper.Map<CityDto>(city);
            cityDto.numberOfHotels = await _unitOfWork.Cities.CountNumberOfHotelsInCityAsync(request.CityID);

            return cityDto;
        }
    }
}
