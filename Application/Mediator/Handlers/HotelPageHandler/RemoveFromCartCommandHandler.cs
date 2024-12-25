using Application.Mediator.Commands.HotelPageCommands;
using Contracts.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.HotelPageHandler
{
    public class RemoveFromCartCommandHandler : IRequestHandler<RemoveFromCartCommand, bool>
    {
        private readonly ICartService _cartService;
        private readonly ILogger<RemoveFromCartCommandHandler> _logger;

        public RemoveFromCartCommandHandler(ICartService cartService, ILogger<RemoveFromCartCommandHandler> logger)
        {
            _cartService = cartService ?? throw new ArgumentNullException(nameof(cartService));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<bool> Handle(RemoveFromCartCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("RemoveFromCartCommandHandler.Handle called");

            var result = _cartService.RemoveFromCart(request.RoomId);

            if (result)
            {
                _logger.LogInformation($"Item with RoomId {request.RoomId} removed from cart.");
            }
            else
            {
                _logger.LogWarning($"Item with RoomId {request.RoomId} not found in cart.");
            }

            return result;
        }
    }
}
