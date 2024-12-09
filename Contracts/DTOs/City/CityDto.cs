namespace Contracts.DTOs.City
{
    public class CityDto : BaseCityDto
    {
        /// <summary>
        /// Gets the number of hotels in the city.
        /// </summary>
        /// <example>25</example>
        public int numberOfHotels { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}
