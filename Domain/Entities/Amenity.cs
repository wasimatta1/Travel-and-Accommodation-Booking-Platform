
namespace Domain.Entities
{
    public class Amenity
    {
        public int AmenitiesID { get; set; }
        public string Name { get; set; }

        //Navigation Properties
        public ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
        public ICollection<HotelAmenity> HotelAmenities { get; set; } = new List<HotelAmenity>();

        public static implicit operator int(Amenity v)
        {
            throw new NotImplementedException();
        }
    }
}
