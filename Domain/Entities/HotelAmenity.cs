namespace Domain.Entities
{
    public class HotelAmenity
    {
        public int HotelID { get; set; }
        public Hotel Hotel { get; set; }
        public int AmenityID { get; set; }
        public Amenity Amenity { get; set; }
    }
}
