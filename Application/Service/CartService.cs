using Contracts.DTOs.HotelPage;
using Contracts.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Service
{
    public class CartService : ICartService
    {
        private readonly ILogger<CartService> _logger;
        private readonly ICollection<AddRoomToCartDto> _cart = new List<AddRoomToCartDto>();

        public CartService(ILogger<CartService> logger)
        {
            _logger = logger;
        }

        public void AddToCart(AddRoomToCartDto item)
        {
            _cart.Add(item);
            _logger.LogInformation($"Item added to cart: {item}");
        }

        public IEnumerable<AddRoomToCartDto> GetCartItems()
        {
            _logger.LogInformation($"Retrieved cart items. Total items: {_cart.Count}");
            return _cart.ToList();
        }

        public bool RemoveFromCart(int roomId)
        {
            var item = _cart.FirstOrDefault(i => i.RoomId == roomId);
            if (item != null)
            {
                _cart.Remove(item);
                _logger.LogInformation($"Item removed from cart: {item}");
                return true;
            }

            _logger.LogWarning($"Attempted to remove non-existing item.RoomId: {roomId}");
            return false;
        }
    }

}
