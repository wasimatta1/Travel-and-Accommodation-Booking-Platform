namespace Contracts.DTOs.Hotel
{
    public class HotelDto : BaseHotelDto
    {

        /// <summary>
        /// Get the name of the hotel's owner.
        /// </summary>
        /// <example>John Doe</example>
        public string OwnerName { get; set; }

        /// <summary>
        /// Get the number of rooms available in the hotel.
        /// </summary>
        /// <example>150</example>
        public int RoomCount { get; set; }

        /// <summary>
        /// Get the City Name of the hotel.
        /// </summary>  
        /// <example>Paris</example>
        public string CityName { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }


    }
}
