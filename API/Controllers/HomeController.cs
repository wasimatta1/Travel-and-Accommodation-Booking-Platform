using Application.Mediator.Queries.HomeQueries;
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
        public async Task<IActionResult> Search(string queryToDestination, string? checkInDate, string? checkOutDate,
             int adults = 2, int children = 0, int rooms = 1, decimal? priceMin = null, int? starRating = null,
             decimal? priceMax = null, string? roomType = null, [FromQuery] string[] amenities = null,
             int pageNumber = 1, int pageSize = 10)
        {

            DateTime today = DateTime.Today;
            DateTime checkIn = today;
            DateTime checkOut = today.AddDays(1);
            if (checkInDate != null)
                checkIn = DateTime.Parse(checkInDate);
            if (checkOutDate != null)
                checkOut = DateTime.Parse(checkOutDate);


            var response = await _mediator.Send(new SearchRequestQurey
            {
                Query = queryToDestination,
                CheckInDate = checkIn,
                CheckOutDate = checkOut,
                Adults = adults,
                Children = children,
                Rooms = rooms,
                PriceMin = priceMin,
                PriceMax = priceMax,
                RoomType = roomType,
                Amenities = amenities,
                StarRating = starRating,
                PageNumber = pageNumber,
                PageSize = pageSize
            });
            return Ok(response);
        }
    }
}
