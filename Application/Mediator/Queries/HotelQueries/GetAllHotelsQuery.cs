using Contracts.DTOs.Hotel;
using MediatR;

namespace Application.Mediator.Queries.HotelQueries
{
    public class GetAllHotelsQuery : IRequest<IEnumerable<HotelDto>>
    {
        public string HotelName { get; set; }
        public string City { get; set; }
        public decimal? StarRating { get; set; }
        public string Owner { get; set; }
        public int PagNumber { get; set; }
        public int PageSize { get; set; }
    }
}
