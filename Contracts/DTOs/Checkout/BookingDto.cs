using Contracts.DTOs.Room;

namespace Contracts.DTOs.Checkout
{
    public class BookingDto
    {
        public RoomDetailsDto RoomDetails { get; set; }
        public decimal TotalPriceForRoom { get; set; }
    }
}
