using Application.Mediator.Commands.CityCommands;
using AutoMapper;
using Contracts.Interfaces.RepositoryInterfaces;
using Domain.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.CityHandler
{
    public class CreateCityCommandHandler : IRequestHandler<CreateCityCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<CreateCityCommandHandler> _logger;

        public CreateCityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<CreateCityCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Handle(CreateCityCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling CreateCityCommand for City: {request.CreateCityDto.Name}");


            var city = _mapper.Map<City>(request.CreateCityDto);
            await _unitOfWork.Cities.CreateAsync(city);
            await _unitOfWork.CompleteAsync();

            _logger.LogInformation($"City: {request.CreateCityDto.Name} created successfully");
            return city.CityID;
        }
    }
}
