namespace Domain.Entities
{
    public class Discount
    {
        public int DiscountID { get; set; }

        public int RoomId { get; set; }

        public decimal DiscountPercentage { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        // Navigation Properties
        public Room Room { get; set; }
    }
}
