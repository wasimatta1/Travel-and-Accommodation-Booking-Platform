namespace Domain.Entities
{
    public class Hotel
    {

        public int HotelID { get; set; }

        public string Name { get; set; }


        public int CityID { get; set; }


        public string OwnerID { get; set; }


        public decimal? StarRating { get; set; }

        public string Description { get; set; }


        public string Address { get; set; }

        public string LocationGoogelMap { get; set; }
        public string ThumbnailURL { get; set; }

        public string ImageURL { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public City City { get; set; }
        public User Owner { get; set; }
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
        public ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();
        public ICollection<HotelAmenity> HotelAmenities { get; set; } = new List<HotelAmenity>();
        public ICollection<Review> Reviews { get; set; } = new List<Review>();
    }
}
