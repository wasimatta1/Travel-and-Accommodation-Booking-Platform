using Contracts.DTOs.Checkout;
using MediatR;

namespace Application.Mediator.Commands.CheckoutCommands
{
    public class ProcessCheckoutCommand : IRequest<BookingConfirmationDto?>
    {
        public CheckoutRequestDto CheckoutInfo { get; set; }

    }
}
