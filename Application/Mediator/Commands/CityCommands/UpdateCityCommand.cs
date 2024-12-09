using Contracts.DTOs.City;
using MediatR;

namespace Application.Mediator.Commands.CityCommands
{
    public class UpdateCityCommand : IRequest<CityDto>
    {
        public UpdateCityDto UpdateCityDto { get; set; }
    }
}
