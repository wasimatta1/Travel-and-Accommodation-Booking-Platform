using Contracts.DTOs.Room;
using MediatR;

namespace Application.Mediator.Commands.RoomCommands
{
    public class UpdateRoomCommand : IRequest<RoomDto>
    {
        public UpdateRoomDto UpdateRoomDto { get; set; }
    }
}
