using Application.Mediator.Commands.CityCommands;
using AutoMapper;
using Contracts.Interfaces.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.CityHandler
{
    public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetCityByIdQueryHandler> _logger;

        public DeleteCityCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetCityByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<int> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteCityCommand for City ID: {CityID}", request.CityID);
            var city = await _unitOfWork.Cities.GetByIdAsync(request.CityID);
            if (city == null)
            {
                _logger.LogWarning("City with ID: {CityID} was not found", request.CityID);
                return 0;
            }
            await _unitOfWork.Cities.DeleteAsync(city);

            _logger.LogInformation("City with ID: {CityID} deleted successfully", request.CityID);

            return await _unitOfWork.CompleteAsync();

        }
    }
}
