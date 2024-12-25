using MediatR;

namespace Application.Mediator.Commands.HotelPageCommands
{
    public class RemoveFromCartCommand : IRequest<bool>
    {
        public int RoomId { get; set; }

    }
}
