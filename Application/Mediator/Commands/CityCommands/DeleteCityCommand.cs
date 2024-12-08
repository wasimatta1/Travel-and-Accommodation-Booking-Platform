using MediatR;

namespace Application.Mediator.Commands.CityCommands
{
    public class DeleteCityCommand : IRequest<int>
    {
        public int CityID { get; set; }
    }
}
