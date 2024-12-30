namespace Contracts.DTOs.Room
{
    public class RoomDetailsDto
    {
        public string RoomNumber { get; set; }
        public string RoomType { get; set; }
        public int AdultsCapacity { get; set; }
        public int ChildrenCapacity { get; set; }
    }
}
