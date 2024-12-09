namespace Contracts.DTOs.Hotel
{
    public class CreateHotelDto : BaseHotelDto
    {
        /// <summary>
        /// set the unique identifier of the city where the hotel is located.
        /// </summary>
        /// <example>1</example>
        public int CityID { get; set; }

        /// <summary>
        /// set the unique identifier of the hotel owner.
        /// </summary>
        /// <example>1</example>
        public string OwnerID { get; set; }
    }
}
