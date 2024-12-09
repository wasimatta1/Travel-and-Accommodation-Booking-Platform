using Application.Mediator.Commands.CityCommands;
using Contracts.Interfaces.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.CityHandler
{
    public class DeleteCityCommandHandler : IRequestHandler<DeleteCityCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteCityCommandHandler> _logger;

        public DeleteCityCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteCityCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<int> Handle(DeleteCityCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling DeleteCityCommand for City ID: {CityID}", request.CityID);
            var city = await _unitOfWork.Cities.GetByIdAsync(request.CityID);
            if (city == null)
            {
                _logger.LogWarning($"City with ID: {request.CityID} was not found");
                return 0;
            }
            await _unitOfWork.Cities.DeleteAsync(city);

            _logger.LogInformation($"City with ID: {request.CityID} deleted successfully");

            return await _unitOfWork.CompleteAsync();

        }
    }
}
