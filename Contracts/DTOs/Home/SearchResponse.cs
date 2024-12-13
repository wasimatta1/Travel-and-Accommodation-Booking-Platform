namespace Contracts.DTOs.Home
{
    public class SearchResponse
    {
        public string HotelName { get; set; }
        public string ThumbnailUrl { get; set; }
        public decimal StarRating { get; set; }
        public string RoomType { get; set; }
        public string HotelDescription { get; set; }
        public decimal PricePerNight { get; set; }
        public decimal? PricePerNightDiscounted { get; set; }

    }
}
