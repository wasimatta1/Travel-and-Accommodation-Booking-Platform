namespace Contracts.DTOs.City
{
    public class BaseCityDto
    {
        /// <summary>
        /// Gets or sets the name of the city.
        /// </summary>
        /// <example>Paris</example>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the country where the city is located.
        /// </summary>
        /// <example>France</example>
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets the URL of the city's thumbnail image.
        /// </summary>
        /// <example>https://www.example.com/images/paris-thumbnail.jpg</example>
        public string ThumbnailURL { get; set; }

        /// <summary>
        /// Gets or sets the post office code of the city.
        /// </summary>
        /// <example>A75</example>
        public string PostOffice { get; set; }
    }
}
