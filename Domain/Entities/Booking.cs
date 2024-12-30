namespace Domain.Entities
{
    public class Booking
    {
        public int BookingID { get; set; }

        public string UserID { get; set; }
        public int PaymentID { get; set; }

        public DateTime CheckInDate { get; set; }

        public DateTime CheckOutDate { get; set; }

        public decimal TotalPrice { get; set; }


        // Navigation Properties
        public User User { get; set; }
        public Payment Payment { get; set; }
        public ICollection<BookingRoom> BookingRooms { get; set; } = new List<BookingRoom>();
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
    }

}
