using Contracts.DTOs.City;
using MediatR;

namespace Application.Mediator.Queries.CityQueries
{
    public class GetAllCitiesQuery : IRequest<IEnumerable<CityDto>>
    {
        public string CityName { get; set; }
        public string Country { get; set; }
        public string PostOffice { get; set; }
        public int PagNumber { get; set; }
        public int PageSize { get; set; }


    }
}
