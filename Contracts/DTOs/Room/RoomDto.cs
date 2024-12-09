namespace Contracts.DTOs.Room
{
    public class RoomDto : BaseRoomDto
    {

        /// <summary>
        /// Gets the name of the hotel.
        /// </summary>
        public string HotelName { get; set; }

        /// <summary>
        /// Get the creation date of the room record.
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// Get the last update date of the room record.
        /// </summary>
        public DateTime? UpdatedAt { get; set; }
    }
}
