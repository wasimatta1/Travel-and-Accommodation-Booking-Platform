using Contracts.DTOs.HotelPage;

namespace Contracts.Interfaces
{
    public interface ICartService
    {
        IEnumerable<AddRoomToCartDto> GetCartItems();
        void AddToCart(AddRoomToCartDto item);
        bool RemoveFromCart(int roomId);
    }
}
