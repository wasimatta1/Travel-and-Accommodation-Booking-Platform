namespace Contracts.DTOs.Hotel
{
    public class BaseHotelDto
    {
        // <summary>
        /// Get or set the name of the hotel.
        /// </summary>
        /// <example>Grand Plaza Hotel</example>
        public string Name { get; set; }


        /// <summary>
        /// Get or set the description of the hotel.
        /// </summary>
        /// <example>A luxurious hotel offering premium services and amenities.</example>
        public string Description { get; set; }

        /// <summary>
        /// Get or set the physical address of the hotel.
        /// </summary>
        /// <example>123 Main Street, Paris, France</example>
        public string Address { get; set; }

        /// <summary>
        /// Get or set the Google Maps location of the hotel.
        /// </summary>>
        /// <example>https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d2624.707187573...</example>
        public string LocationGoogelMap { get; set; }

        /// <summary>
        /// Get or set the URL of the hotel's thumbnail image.
        /// </summary>
        /// <example>https://www.example.com/images/hotels/grand-plaza-thumbnail.jpg</example>
        public string ThumbnailURL { get; set; }

        /// <summary>
        /// Get or set the URL of the hotel's main image.
        /// </summary>
        /// <example>https://www.example.com/images/hotels/grand-plaza-main.jpg</example>
        public string ImageURL { get; set; }
    }
}
