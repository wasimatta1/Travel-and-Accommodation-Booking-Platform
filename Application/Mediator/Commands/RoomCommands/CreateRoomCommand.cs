using Contracts.DTOs.Room;
using MediatR;

namespace Application.Mediator.Commands.RoomCommands
{
    public class CreateRoomCommand : IRequest<int>
    {
        public CreateRoomDto CreateRoomDto { get; set; }
    }
}
