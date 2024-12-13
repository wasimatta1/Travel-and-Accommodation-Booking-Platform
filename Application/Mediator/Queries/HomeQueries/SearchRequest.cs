using Contracts.DTOs.HomeDto;
using MediatR;

namespace Application.Mediator.Queries.HomeQueries
{
    public class SearchRequest : IRequest<SearchResponse>
    {
        public string Query { get; set; }
        public DateTime CheckInDate { get; set; } = DateTime.Now;
        public DateTime CheckOutDate { get; set; } = DateTime.Now.AddDays(1);
        public int Adults { get; set; } = 2;
        public int Children { get; set; } = 0;
        public int Rooms { get; set; } = 1;
    }
}
