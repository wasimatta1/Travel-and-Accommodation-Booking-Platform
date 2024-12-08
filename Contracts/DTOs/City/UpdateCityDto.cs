namespace Contracts.DTOs.City
{
    public class UpdateCityDto : BaseCityDto
    {
        /// <summary>
        ///  sets the unique identifier of the city.
        /// </summary>
        /// <example>1</example>
        public int CityID { get; set; }
    }
}
