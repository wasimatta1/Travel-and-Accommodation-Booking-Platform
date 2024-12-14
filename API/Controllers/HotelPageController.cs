using Application.Mediator.Queries.HotelPageQueries;
using Contracts.DTOs.HotelPage;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelPageController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HotelPageController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        /// <summary>
        /// Retrieves hotel page details including available rooms, based on the specified search criteria.
        /// </summary>
        /// <param name="hotelId">The unique identifier for the hotel to search for.</param>
        /// <param name="checkInDate">Optional check-in date. Defaults to today's date if not provided.</param>
        /// <param name="checkOutDate">Optional check-out date. Defaults to one day after the check-in date if not provided.</param>
        /// <param name="adults">The number of adults for the room. Defaults to 2.</param>
        /// <param name="children">The number of children for the room. Defaults to 0.</param>
        /// <param name="priceMin">Optional minimum price per night for filtering the available rooms.</param>
        /// <param name="priceMax">Optional maximum price per night for filtering the available rooms.</param>
        /// <param name="roomType">Optional room type for filtering the available rooms.</param>
        /// <returns>A hotel page including room availability based on the provided criteria.</returns>
        /// <response code="200">Returns the details of the hotel and available rooms matching the search criteria.</response>
        [HttpGet]
        [ProducesResponseType(typeof(HotelPageDto), 200)]
        public async Task<ActionResult> GetHotelPage(int hotelId, string? checkInDate, string? checkOutDate,
         int adults = 2, int children = 0, decimal? priceMin = null
            , decimal? priceMax = null, string? roomType = null)
        {
            DateTime today = DateTime.Today;
            DateTime checkIn = today;
            DateTime checkOut = today.AddDays(1);
            if (checkInDate != null)
                checkIn = DateTime.Parse(checkInDate);
            if (checkOutDate != null)
                checkOut = DateTime.Parse(checkOutDate);

            var response = await _mediator.Send(new GetHotelPageQuery
            {
                HotelId = hotelId,
                CheckInDate = checkIn,
                CheckOutDate = checkOut,
                Adults = adults,
                Children = children,
                PriceMin = priceMin,
                PriceMax = priceMax,
                RoomType = roomType
            });
            return Ok(response);
        }
    }
}
