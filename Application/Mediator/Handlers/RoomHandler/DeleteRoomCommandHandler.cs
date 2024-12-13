using Application.Mediator.Commands.RoomCommands;
using Contracts.Interfaces.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.RoomHandler
{
    public class DeleteRoomCommandHandler : IRequestHandler<DeleteRoomCommand, int>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<DeleteRoomCommandHandler> _logger;

        public DeleteRoomCommandHandler(IUnitOfWork unitOfWork, ILogger<DeleteRoomCommandHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<int> Handle(DeleteRoomCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling DeleteRoomCommand for Room ID: {request.RoomID}");
            var room = await _unitOfWork.Rooms.GetByIdAsync(request.RoomID);
            if (room == null)
            {
                _logger.LogWarning($"Room with ID: {request.RoomID} was not found");
                return 0;
            }
            await _unitOfWork.Rooms.DeleteAsync(room);

            _logger.LogInformation($"Room with ID: {request.RoomID} deleted successfully");

            return await _unitOfWork.CompleteAsync();
        }
    }
}