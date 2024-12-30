using Application.Mediator.Commands.CheckoutCommands;
using Application.Mediator.Queries.CheckoutQueries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "User")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CheckoutController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Processes the checkout request and generates the booking confirmation.
        /// </summary>
        /// <param name="processCheckoutCommand">The details of the checkout request.</param>
        /// <returns>A response indicating the success or failure of the checkout process.</returns>
        /// <response code="200">Indicates that the checkout was successfully processed.</response>
        /// <response code="400">If the checkout request is invalid or fails.</response>
        [HttpPost("process")]
        [ProducesResponseType(typeof(ProcessCheckoutCommand), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ProcessCheckout(ProcessCheckoutCommand processCheckoutCommand)
        {
            var response = await _mediator.Send(processCheckoutCommand);
            if (response != null)
                return Ok(response);

            return BadRequest();
        }

        /// <summary>
        /// Downloads the booking confirmation as a PDF.
        /// </summary>
        /// <returns>The booking confirmation PDF file.</returns>
        /// <response code="200">Returns the booking confirmation PDF file.</response>
        /// <response code="400">If the PDF generation fails.</response>
        [HttpGet("download-booking-confirmation")]
        [ProducesResponseType(typeof(byte[]), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DownloadBookingConfirmation()
        {
            var pdfData = await _mediator.Send(new GenerateBookingConfirmationPDFQueries());
            if (pdfData == null)
                return BadRequest("Failed to generate PDF.");

            return File(pdfData, "application/pdf", "BookingConfirmation.pdf");
        }

    }
}
