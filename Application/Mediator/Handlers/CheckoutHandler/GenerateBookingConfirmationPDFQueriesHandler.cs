using Application.Mediator.Queries.CheckoutQueries;
using Contracts.DTOs.Checkout;
using Contracts.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.CheckoutHandler
{
    public class GenerateBookingConfirmationPDFQueriesHandler : IRequestHandler<GenerateBookingConfirmationPDFQueries, byte[]?>
    {
        private readonly ICacheService _cacheService;
        private readonly IPdfGeneratorService _pdfGeneratorService;
        private readonly ILogger<GenerateBookingConfirmationPDFQueriesHandler> _logger;

        public GenerateBookingConfirmationPDFQueriesHandler(ILogger<GenerateBookingConfirmationPDFQueriesHandler> logger,
            ICacheService cacheService,
            IPdfGeneratorService pdfGeneratorService)
        {
            _logger = logger;
            _cacheService = cacheService;
            _pdfGeneratorService = pdfGeneratorService;
        }

        public async Task<byte[]?> Handle(GenerateBookingConfirmationPDFQueries request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GenerateBookingConfirmationPDFQueriesHandler.Handle called");

            var confirmation = _cacheService.Get<BookingConfirmationDto>("bookingConfirmationDto");
            if (confirmation == null)
            {
                _logger.LogWarning("No booking confirmation found in cache.");
                return null;
            }
            var pdfData = _pdfGeneratorService.GenerateBookingConfirmationPdf(confirmation);
            return pdfData;
        }
    }
}
