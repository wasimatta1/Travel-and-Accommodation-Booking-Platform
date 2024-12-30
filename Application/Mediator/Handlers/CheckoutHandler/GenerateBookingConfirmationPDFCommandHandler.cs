using Application.Mediator.Commands.CheckoutCommands;
using Contracts.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.CheckoutHandler
{
    public class GenerateBookingConfirmationPDFCommandHandler : IRequestHandler<GenerateBookingConfirmationPDFCommand, byte[]?>
    {
        private readonly IPdfGeneratorService _pdfGeneratorService;
        private readonly ILogger<GenerateBookingConfirmationPDFCommandHandler> _logger;

        public GenerateBookingConfirmationPDFCommandHandler(ILogger<GenerateBookingConfirmationPDFCommandHandler> logger,
            IPdfGeneratorService pdfGeneratorService)
        {
            _logger = logger;
            _pdfGeneratorService = pdfGeneratorService;
        }

        public async Task<byte[]?> Handle(GenerateBookingConfirmationPDFCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("GenerateBookingConfirmationPDFQueriesHandler.Handle called");
            var pdfData = _pdfGeneratorService.GenerateBookingConfirmationPdf(request.BookingConfirmationDto);
            return pdfData;
        }
    }
}
