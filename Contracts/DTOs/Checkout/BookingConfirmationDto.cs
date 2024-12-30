namespace Contracts.DTOs.Checkout
{
    public class BookingConfirmationDto
    {
        public string HotelName { get; set; }
        public string ConfirmationNumber { get; set; }
        public string HotelAddress { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }
        public decimal TotalPrice { get; set; }
        public IList<BookingDto> BookingDtos { get; set; } = new List<BookingDto>();
    }
}
