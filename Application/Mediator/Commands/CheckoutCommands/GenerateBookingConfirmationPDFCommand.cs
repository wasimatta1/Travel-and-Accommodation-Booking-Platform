using Contracts.DTOs.Checkout;
using MediatR;

namespace Application.Mediator.Commands.CheckoutCommands
{
    public class GenerateBookingConfirmationPDFCommand : IRequest<byte[]?>
    {
        public BookingConfirmationDto BookingConfirmationDto { get; set; }
    }
}
