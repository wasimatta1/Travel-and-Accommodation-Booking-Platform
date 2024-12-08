using Contracts.DTOs.City;
using MediatR;

namespace Application.Mediator.Queries.CityQueries
{
    public class GetCityByIdQuery : IRequest<CityDto>
    {
        public int CityID { get; set; }
    }
}
