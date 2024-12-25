namespace Contracts.DTOs.HotelPage
{
    public class AddRoomToCartDto
    {
        public int RoomId { get; set; }
        public DateTime CheckInDate { get; set; }
        public DateTime CheckOutDate { get; set; }

    }

}
