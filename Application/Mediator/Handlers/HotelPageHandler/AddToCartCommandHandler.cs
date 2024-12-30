using Application.Mediator.Commands.HotelPageCommands;
using Contracts.Interfaces;
using Contracts.Interfaces.RepositoryInterfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.HotelPageHandler
{
    public class AddToCartCommandHandler : IRequestHandler<AddToCartCommand, bool>
    {
        private readonly ICartService _cartService;
        private readonly ILogger<AddToCartCommandHandler> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public AddToCartCommandHandler(ICartService cartService, ILogger<AddToCartCommandHandler> logger, IUnitOfWork unitOfWork)
        {
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _unitOfWork = unitOfWork;
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
            var firstItem = cartItems.FirstOrDefault();
            if (firstItem != null)
            {
                var requstedRoom = await _unitOfWork.Rooms.GetByIdAsync(request.CartItem.RoomId);
                var firstRoom = await _unitOfWork.Rooms.GetByIdAsync(firstItem.RoomId);
                if (requstedRoom!.HotelID != firstRoom!.HotelID)
                {
                    _logger.LogWarning($"Attempted to add an item from a different hotel.");
                    return false;
                }
            }

            _cartService.AddToCart(request.CartItem);

            _logger.LogInformation("AddToCartCommandHandler.Handle returned");

            return true;
        }
    }
}
