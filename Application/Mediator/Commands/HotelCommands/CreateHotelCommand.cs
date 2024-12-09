using Contracts.DTOs.Hotel;
using MediatR;


namespace Application.Mediator.Commands.HotelCommands
{
    public class CreateHotelCommand : IRequest<int>
    {
        public CreateHotelDto CreateHotelDto { get; set; }
    }
}
