﻿namespace Domain.Entities
{
    public class Hotel
    {

        public int HotelID { get; set; }

        public string Name { get; set; }


        public int CityID { get; set; }


        public string OwnerID { get; set; }


        public decimal StarRating { get; set; }

        public string Description { get; set; }


        public string Address { get; set; }
        public string ThumbnailURL { get; set; }

        public string ImageURL { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }

        // Navigation Properties
        public City City { get; set; }
        public User Owner { get; set; }
        public ICollection<Room> Rooms { get; set; } = new List<Room>();

    }
}