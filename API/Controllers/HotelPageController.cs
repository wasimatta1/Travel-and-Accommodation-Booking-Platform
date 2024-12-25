using Application.Mediator.Commands.HotelPageCommands;
using Application.Mediator.Queries.HotelPageQueries;
using Contracts.DTOs.HotelPage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        /// <summary>
        /// Adds an item to the cart based on the provided details.
        /// </summary>
        /// <param name="cartItem">The details of the cart item to be added.</param>
        /// <returns>A confirmation message indicating the item was added successfully.</returns>
        /// <response code="200">Item successfully added to the cart.</response>
        /// <response code="400">Invalid cart item provided.</response>
        [HttpPost("add-to-cart")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> AddToCart(AddRoomToCartDto cartItem)
        {
            var response = await _mediator.Send(new AddToCartCommand
            {
                CartItem = cartItem
            });
            if (!response)
                return BadRequest("Room already in the cart.");
            return Ok("Room added to cart successfully.");
        }

        /// <summary>
        /// Retrieves all items currently in the cart.
        /// </summary>
        /// <returns>A list of items in the cart.</returns>
        /// <response code="200">Returns the list of cart items.</response>
        [HttpGet("get-cart-rooms")]
        [ProducesResponseType(typeof(IEnumerable<AddRoomToCartDto>), 200)]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> GetCartItems()
        {
            var response = await _mediator.Send(new GetCartItemsQuery());
            return Ok(response);
        }

        /// <summary>
        /// Removes an item from the cart based on the specified room ID.
        /// </summary>
        /// <param name="roomId">The unique identifier of the room to be removed from the cart.</param>
        /// <returns>A confirmation message if the item was removed successfully, or an error message if not found.</returns>
        /// <response code="200">Item successfully removed from the cart.</response>
        /// <response code="404">Item not found in the cart.</response>
        [HttpDelete("remove-from-cart")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> RemoveFromCart(int roomId)
        {
            var response = await _mediator.Send(new RemoveFromCartCommand
            {
                RoomId = roomId
            });
            if (!response)
                return NotFound("Room not found in the cart.");
            return Ok("Room removed from cart successfully.");
        }


    }
}
