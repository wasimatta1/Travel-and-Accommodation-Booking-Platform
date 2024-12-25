namespace Contracts.DTOs.HotelPage
{
    public class CartItemDto
    {
        public string RoomNumber { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal PricePerNight { get; set; }
        public decimal? PricePerNightDescounted { get; set; }
        public decimal TotalPrice { get; set; }
        public decimal? DescountedTotalPrice { get; set; }
    }

}
