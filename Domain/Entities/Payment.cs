namespace Domain.Entities
{
    public class Payment
    {
        public int PaymentID { get; set; }
        public int BookingID { get; set; }
        public string PaymentMethod { get; set; }
        public string? SpecialRequests { get; set; }
        public decimal TotalPrice { get; set; }
        public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

        //Navigation Properties
        public Booking Booking { get; set; }
    }
}
