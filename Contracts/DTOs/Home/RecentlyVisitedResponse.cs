namespace Contracts.DTOs.Home
{
    public class RecentlyVisitedResponse
    {

        public string HotelName { get; set; }
        public string ThumbnailUrl { get; set; }
        public decimal StarRating { get; set; }
        public string City { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
