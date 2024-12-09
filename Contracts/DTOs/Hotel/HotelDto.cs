namespace Contracts.DTOs.Hotel
{
    public class HotelDto : BaseHotelDto
    {
        /// <summary>
        /// Get the star rating of the hotel.
        /// </summary>
        /// <example>4</example>
        public decimal StarRating { get; set; }

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


    }
}
