using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HomeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("search")]
        public async Task<IActionResult> Search(string queryToDestination, string CheckInDate, string checkOutDate,
             int adults = 2, int children = 0, int rooms = 1, decimal? priceMin = null,
             decimal? priceMax = null, string? Type = null, [FromQuery] string[] amenities = null)
        {
            DateTime today = DateTime.Today;
            DateTime checkIn = today;
            DateTime checkOut = today.AddDays(1);
            //var response = await _mediator.Send(new SearchRequest { Query = query });
            return Ok();
        }
    }
}
