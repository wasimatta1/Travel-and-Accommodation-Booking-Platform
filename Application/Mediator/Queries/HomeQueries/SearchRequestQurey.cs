using Contracts.DTOs.Home;
using MediatR;

namespace Application.Mediator.Queries.HomeQueries
{
    public class SearchRequestQurey : IRequest<IEnumerable<SearchResultDto>>
    {
        public string Query { get; set; }
        public DateTime CheckInDate { get; set; } = DateTime.Now;
        public DateTime CheckOutDate { get; set; } = DateTime.Now.AddDays(1);
        public int Adults { get; set; } = 2;
        public int Children { get; set; } = 0;
        public int Rooms { get; set; } = 1;
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        //optional filters
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
        public int? StarRating { get; set; }
        public string RoomType { get; set; }
        public string[] Amenities { get; set; }
    }
}
