using Application.Mediator.Commands.CityCommands;
using AutoMapper;
using Contracts.DTOs.City;
using Contracts.Interfaces.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.CityHandler
{
    public class UpdateCityCommandHandler : IRequestHandler<UpdateCityCommand, CityDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<UpdateCityCommandHandler> _logger;

        public UpdateCityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateCityCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CityDto> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling UpdateCityCommand for City ID: {request.UpdateCityDto.CityID}");

            var city = await _unitOfWork.Cities.GetByIdAsync(request.UpdateCityDto.CityID);
            if (city == null)
            {
                _logger.LogWarning($"City with ID: {request.UpdateCityDto.CityID} was not found");
                return null;
            }

            _mapper.Map(request.UpdateCityDto, city);
            city.UpdatedAt = DateTime.Now;

            var updatedCity = await _unitOfWork.Cities.UpdateAsync(city);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation($"City with ID: {city.CityID} updated successfully.");

            return _mapper.Map<CityDto>(city);
        }
    }
}
