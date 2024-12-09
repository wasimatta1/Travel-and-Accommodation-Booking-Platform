namespace Domain.Entities
{
    public class City
    {

        public int CityID { get; set; }


        public string Name { get; set; }


        public string Country { get; set; }

        public string ThumbnailURL { get; set; }

        public string PostOffice { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
    }
}
