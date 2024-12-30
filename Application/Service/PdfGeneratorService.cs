using Contracts.DTOs.Checkout;
using Contracts.Interfaces;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace Application.Service
{
    public class PdfGeneratorService : IPdfGeneratorService
    {
        public byte[] GenerateBookingConfirmationPdf(BookingConfirmationDto confirmation)
        {

            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new PdfWriter(memoryStream))
                {
                    using (var pdfDocument = new PdfDocument(writer))
                    {
                        var document = new Document(pdfDocument);

                        document.Add(new Paragraph("Booking Confirmation")
                            .SetFontSize(18));

                        document.Add(new Paragraph($"Hotel Name: {confirmation.HotelName}"));
                        document.Add(new Paragraph($"Hotel Address: {confirmation.HotelAddress}"));
                        document.Add(new Paragraph($"Check-in Date: {confirmation.CheckInDate:yyyy-MM-dd}"));
                        document.Add(new Paragraph($"Check-out Date: {confirmation.CheckOutDate:yyyy-MM-dd}"));
                        document.Add(new Paragraph($"Total Price: ${confirmation.TotalPrice:F2}"));
                        document.Add(new Paragraph(" "));

                        document.Add(new Paragraph("Room Details:"));

                        foreach (var bookingDto in confirmation.BookingDtos)
                        {
                            document.Add(new Paragraph($"Room Number: {bookingDto.RoomDetails.RoomNumber}"));
                            document.Add(new Paragraph($"Room Type: {bookingDto.RoomDetails.RoomType}"));
                            document.Add(new Paragraph($"Total Price for Room: ${bookingDto.TotalPriceForRoom:F2}"));
                            document.Add(new Paragraph(" ")); // Add space between room details
                        }

                        document.Add(new Paragraph("Thank you for your booking!"));
                    }
                }

                return memoryStream.ToArray();
            }
        }
    }
}
