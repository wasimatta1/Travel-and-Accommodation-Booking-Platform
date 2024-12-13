using Domain.Enums;

namespace Contracts.DTOs.HomeDto
{
    public class SearchResponse
    {
        public string HotelName { get; set; }
        public string ThumbnailUrl { get; set; }
        public decimal StarRating { get; set; }
        public RoomType RoomType { get; set; }
        public string RoomDescription { get; set; }

        public decimal PricePerNight { get; set; }

    }
}
