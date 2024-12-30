using Application.Mediator.Commands.CheckoutCommands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckoutController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CheckoutController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("process")]
        public async Task<IActionResult> ProcessCheckout(ProcessCheckoutCommand processCheckoutCommand)
        {
            var response = await _mediator.Send(processCheckoutCommand);
            if (response != null)
                return Ok(response);

            return BadRequest();
        }
    }
}
