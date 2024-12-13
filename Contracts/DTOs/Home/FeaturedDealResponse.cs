namespace Contracts.DTOs.Home
{
    public class FeaturedDealResponse
    {
        public string HotelName { get; set; }
        public string Location { get; set; }
        public string ThumbnailUrl { get; set; }
        public decimal StarRating { get; set; }
        public decimal PricePerNight { get; set; }
        public decimal? PricePerNightDiscounted { get; set; }
    }
}
