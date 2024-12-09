using MediatR;


namespace Application.Mediator.Commands.HotelCommands
{
    public class DeleteHotelCommand : IRequest<int>
    {
        public int HotelID { get; set; }
    }
}
