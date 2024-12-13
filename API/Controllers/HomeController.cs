using Application.Mediator.Queries.HomeQueries;
using Contracts.DTOs.Home;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Searches for available rooms and hotels based on provided search criteria.
        /// </summary>
        /// <param name="queryToDestination">The destination or query to search for.</param>
        /// <param name="checkInDate">Optional check-in date. Defaults to today's date if not provided.</param>
        /// <param name="checkOutDate">Optional check-out date. Defaults to one day after today's date if not provided.</param>
        /// <param name="adults">The number of adults. Defaults to 2.</param>
        /// <param name="children">The number of children. Defaults to 0.</param>
        /// <param name="rooms">The number of rooms. Defaults to 1.</param>
        /// <param name="priceMin">Optional minimum price for filtering.</param>
        /// <param name="priceMax">Optional maximum price for filtering.</param>
        /// <param name="starRating">Optional star rating for filtering.</param>
        /// <param name="roomType">Optional room type for filtering.</param>
        /// <param name="amenities">Optional array of amenities for filtering.</param>
        /// <param name="pageNumber">The page number for pagination. Defaults to 1.</param>
        /// <param name="pageSize">The number of results per page. Defaults to 10.</param>
        /// <returns>A list of available rooms and hotels matching the criteria.</returns>
        /// <response code="200">Returns the list of matching rooms and hotels.</response>
        [HttpGet("search")]
        [ProducesResponseType(typeof(IEnumerable<SearchResultDto>), 200)]
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

        /// <summary>
        /// Retrieves a list of featured deals for rooms and hotels.
        /// </summary>
        /// <param name="take">The number of featured deals to retrieve.</param>
        /// <returns>A list of featured deals.</returns>
        /// <response code="200">Returns the list of featured deals.</response>
        [HttpGet("FeaturedDeals")]
        [ProducesResponseType(typeof(IEnumerable<FeaturedDealDto>), 200)]
        public async Task<IActionResult> GetFeaturedDeals(int take)
        {
            var response = await _mediator.Send(new GetFeaturedDealsQuery()
            {
                Take = take
            });
            return Ok(response);
        }


        /// <summary>
        /// Retrieves a list of recently visited destinations for the logged-in user.
        /// </summary>
        /// <param name="take">The number of recently visited destinations to retrieve.</param>
        /// <returns>A list of recently visited destinations.</returns>
        /// <response code="200">Returns the list of recently visited destinations.</response>
        /// <response code="401">Unauthorized if the user is not logged in.</response>
        [HttpGet("RecentlyVisited")]
        [Authorize(Roles = "User")]
        [ProducesResponseType(typeof(IEnumerable<RecentlyVisitedDto>), 200)]
        [ProducesResponseType(401)]
        public async Task<IActionResult> GetRecentlyVisited(int take)
        {
            var response = await _mediator.Send(new GetRecentlyVisitedQuery()
            {
                Take = take
            });
            return Ok(response);
        }

        /// <summary>
        /// Retrieves a list of trending destinations.
        /// </summary>
        /// <param name="take">The number of trending destinations to retrieve. Defaults to 5.</param>
        /// <returns>A list of trending destinations.</returns>
        /// <response code="200">Returns the list of trending destinations.</response>
        [HttpGet("TrendingDestinations")]
        [ProducesResponseType(typeof(IEnumerable<TrendingDestinationDto>), 200)]
        public async Task<IActionResult> GetTrendingDestinations(int take = 5)
        {
            var response = await _mediator.Send(new GetTrendingDestinationsQuery()
            {
                Take = take
            });
            return Ok(response);
        }
    }
}
