using Contracts.DTOs.Checkout;

namespace Contracts.Interfaces
{
    public interface IPdfGeneratorService
    {
        byte[] GenerateBookingConfirmationPdf(BookingConfirmationDto confirmation);
    }
}
