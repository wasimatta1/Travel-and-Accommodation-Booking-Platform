namespace Domain.Entities
{
    public class Review
    {
        public int ReviewId { get; set; }
        public string? Content { get; set; }
        public int Rating { get; set; }
        public DateTime ReviewDate { get; set; }
        public int HotelId { get; set; }
        public string UserId { get; set; }

        // Navigation properties
        public Hotel Hotel { get; set; }
        public User User { get; set; }
    }
}
