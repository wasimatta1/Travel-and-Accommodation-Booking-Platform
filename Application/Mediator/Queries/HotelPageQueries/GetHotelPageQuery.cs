using Contracts.DTOs.HotelPage;
using MediatR;

namespace Application.Mediator.Queries.HotelPageQueries
{
    public class GetHotelPageQuery : IRequest<HotelPageDto>
    {
        public int HotelId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public int Adults { get; set; }
        public int Children { get; set; }
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
        public string? RoomType { get; set; }
    }
}
