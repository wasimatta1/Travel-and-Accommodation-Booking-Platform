using Contracts.DTOs.HotelPage;
using MediatR;

namespace Application.Mediator.Commands.HotelPageCommands
{
    public class AddToCartCommand : IRequest<bool>
    {
        public AddRoomToCartDto CartItem { get; set; }

    }
}
