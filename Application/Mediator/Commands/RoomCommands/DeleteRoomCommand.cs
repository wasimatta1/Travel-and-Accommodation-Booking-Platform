using MediatR;

namespace Application.Mediator.Commands.RoomCommands
{
    public class DeleteRoomCommand : IRequest<int>
    {
        public int RoomID { get; set; }
    }
}
