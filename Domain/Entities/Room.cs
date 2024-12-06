using Domain.Enums;

namespace Domain.Entities
{
    public class Room
    {

        public int RoomID { get; set; }


        public int HotelID { get; set; }


        public string RoomNumber { get; set; }


        public RoomType RoomType { get; set; }

        public int AdultsCapacity { get; set; }

        public int ChildrenCapacity { get; set; }


        public decimal PricePerNight { get; set; }

        public string Description { get; set; }

        public bool Availability { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public Hotel Hotel { get; set; }
        public ICollection<RoomImage> RoomImages { get; set; } = new List<RoomImage>();

    }
}
