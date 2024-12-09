namespace Contracts.DTOs.Room
{

    public class BaseRoomDto
    {
        /// <summary>
        /// Get or set the room number.
        /// </summary>
        public string RoomNumber { get; set; }

        /// <summary>
        /// Get or set the type of the room.
        /// </summary>
        public String RoomType { get; set; }

        /// <summary>
        /// Get or set the capacity for adults.
        /// </summary>
        public int AdultsCapacity { get; set; }

        /// <summary>
        /// Get or set the capacity for children.
        /// </summary>
        public int ChildrenCapacity { get; set; }

        /// <summary>
        /// Get or set the price per night.
        /// </summary>
        public decimal PricePerNight { get; set; }

        /// <summary>
        /// Get or ses the description of the room.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Get or set the availability status of the room.
        /// </summary>
        public bool Availability { get; set; }

        /// <summary>
        ///  Get or set the Room images URL.
        /// </summary>
        public ICollection<string> ImagesUrl { get; set; } = new List<string>();
    }
}
