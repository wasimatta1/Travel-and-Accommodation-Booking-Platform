using Contracts.DTOs.Hotel;
using MediatR;


namespace Application.Mediator.Commands.HotelCommands
{
    public class UpdateHotelCommand : IRequest<HotelDto?>
    {
        public UpdateHotelDto UpdateHotelDto { get; set; }
    }
}
