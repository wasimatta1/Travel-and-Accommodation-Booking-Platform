using Application.Mediator.Commands.HotelPageCommands;
using Contracts.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.HotelPageHandler
{
    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, bool>
    {
        private readonly ICartService _cartService;
        private readonly ILogger<AddToCartCommandHandler> _logger;

        public AddToCartCommandHandler(ICartService cartService, ILogger<AddToCartCommandHandler> logger)
        {
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(AddToCartCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("AddToCartCommandHandler.Handle called");

            var cartItems = _cartService.GetCartItems();

            var item = cartItems.FirstOrDefault(i => i.RoomId == request.CartItem.RoomId);

            if (item != null)
            {
                _logger.LogWarning($"Attempted to add an item that is already in the cart. RoomId: {request.CartItem.RoomId}");
                return false;
            }

            _cartService.AddToCart(request.CartItem);

            _logger.LogInformation("AddToCartCommandHandler.Handle returned");

            return true;
        }
    }
}
