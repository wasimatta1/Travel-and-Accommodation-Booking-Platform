namespace Contracts.DTOs.HotelPage
{
    public class HotelPageDto
    {
        public string ImageUrl { get; set; }
        public string HotelName { get; set; }
        public int StarRating { get; set; }
        public string Description { get; set; }
        public string? GuestReviewsAVG { get; set; }
        public string? ReviewSample { get; set; }
        public string LocationGooleMap { get; set; }
        public List<RoomPageDto> Rooms { get; set; }

    }

}
