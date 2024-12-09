using Application.Mediator.Queries.RoomQueries;
using AutoMapper;
using Contracts.DTOs.Room;
using Contracts.Interfaces.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.RoomHandler
{
    public class GetRoomByIdQueryHandler : IRequestHandler<GetRoomByIdQuery, RoomDto>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<GetRoomByIdQueryHandler> _logger;

        public GetRoomByIdQueryHandler(IUnitOfWork unitOfWork, IMapper mapper, ILogger<GetRoomByIdQueryHandler> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<RoomDto> Handle(GetRoomByIdQuery request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling GetRoomByIdQuery for Room ID: {request.RoomID}");

            var includes = new[]
            {
                "Hotel",
                "RoomImages"
            };

            var room = await _unitOfWork.Rooms.FindAsync(c => c.RoomID == request.RoomID, includes);

            if (room == null)
            {
                _logger.LogWarning($"Room with ID: {request.RoomID} was not found");
                return null;
            }
            return _mapper.Map<RoomDto>(room);
        }
    }
}
