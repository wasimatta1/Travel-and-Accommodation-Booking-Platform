using Contracts.DTOs.City;
using MediatR;

namespace Application.Mediator.Commands.CityCommands
{
    public class CreateCityCommand : IRequest<int>
    {
        public CreateCityDto CreateCityDto { get; set; }
    }
}
