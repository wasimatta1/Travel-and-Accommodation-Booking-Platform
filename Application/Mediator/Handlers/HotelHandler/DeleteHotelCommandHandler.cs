using Application.Mediator.Commands.HotelCommands;
using Contracts.Interfaces.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.HotelHandler
{
    public class DeleteHotelCommandHandler : IRequestHandler<DeleteHotelCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteHotelCommandHandler> _logger;

        public DeleteHotelCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteHotelCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<int> Handle(DeleteHotelCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling DeleteHotelCommand for Hotel ID: {request.HotelID}");
            var hotel = await _unitOfWork.Hotels.GetByIdAsync(request.HotelID);
            if (hotel == null)
            {
                _logger.LogWarning($"Hotel with ID: {request.HotelID} was not found");
                return 0;
            }
            await _unitOfWork.Hotels.DeleteAsync(hotel);

            _logger.LogInformation($"Hotel with ID: {request.HotelID} deleted successfully");

            return await _unitOfWork.CompleteAsync();

        }
    }
}
