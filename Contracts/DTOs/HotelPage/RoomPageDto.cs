namespace Contracts.DTOs.HotelPage
{
    public class RoomPageDto
    {
        public string RoomType { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal? DiscountedPrice { get; set; }
        public ICollection<string> ImagesUrl { get; set; } = new List<string>();

    }
}
