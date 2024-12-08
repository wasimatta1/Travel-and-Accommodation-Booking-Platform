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
        private readonly ILogger<GetCityByIdQueryHandler> _logger;

        public UpdateCityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetCityByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<CityDto> Handle(UpdateCityCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling UpdateCityCommand for City ID: {CityID}", request.UpdateCityDto.CityID);

            var city = await _unitOfWork.Cities.GetByIdAsync(request.UpdateCityDto.CityID);
            if (city == null)
            {
                _logger.LogWarning("City with ID: {CityID} was not found", request.UpdateCityDto.CityID);
                return null;
            }

            _mapper.Map(request.UpdateCityDto, city);
            city.UpdatedAt = DateTime.Now;

            var updatedCity = await _unitOfWork.Cities.UpdateAsync(city);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation("City with ID: {CityID} updated successfully.", city.CityID);

            return _mapper.Map<CityDto>(city);
        }
    }
}
