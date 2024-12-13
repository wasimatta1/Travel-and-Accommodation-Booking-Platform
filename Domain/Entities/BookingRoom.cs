namespace Domain.Entities
{
    public class BookingRoom
    {

        public int BookingID { get; set; }

        public int RoomID { get; set; }


        // Navigation Properties
        public Booking Booking { get; set; }
        public Room Room { get; set; }
    }
}
