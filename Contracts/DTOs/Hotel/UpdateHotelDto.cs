namespace Contracts.DTOs.Hotel
{
    public class UpdateHotelDto : BaseHotelDto
    {
        /// <summary>
        /// set the unique identifier of the hotel to update.
        /// </summary>
        /// <example>1</example>
        public int HotelID { get; set; }

        /// <summary>
        /// set the unique identifire of the new Owner.
        /// </summary>
        /// <example>1</example>
        public string OwnerID { get; set; }

        /// <summary>
        /// set the unique identifier of the city where the hotel is located.
        /// </summary>
        /// <example>1</example>
        public int CityID { get; set; }
    }
}
