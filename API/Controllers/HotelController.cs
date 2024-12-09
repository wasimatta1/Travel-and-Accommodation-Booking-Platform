using Application.Mediator.Commands.HotelCommands;
using Application.Mediator.Queries.HotelQueries;
using Contracts.DTOs.Hotel;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IMediator _mediator;

        public HotelController(IMediator mediator, ILogger<HotelController> logger)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Retrieves a paginated list of hotels with optional filtering by hotel name, city, owner name, and star rating.
        /// </summary>
        /// <param name="pagNumber">The page number for pagination .</param>
        /// <param name="pageSize">The number of records per page .</param>
        /// <param name="HotelName">Optional filter to search hotels by name.</param>
        /// <param name="City">Optional filter to search hotels by city .</param>
        /// <param name="Owner">Optional filter to search hotels by owner name .</param>
        /// <param name="StarRating">Optional filter to search hotels by star rating .</param>
        /// <returns>A paginated list of hotels matching the specified filters.</returns>
        /// <response code="200">Returns the list of hotels.</response>
        /// <response code="400">If the request parameters are invalid.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<HotelDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllAsync(int pagNumber = 1, int pageSize = 10,
            string HotelName = null, string City = null, string Owner = null, decimal? StarRating = null)
        {

            var query = new GetAllHotelsQuery
            {
                HotelName = HotelName,
                City = City,
                Owner = Owner,
                StarRating = StarRating,
                PagNumber = pagNumber,
                PageSize = pageSize,
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves detailed information about a specific hotel by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the hotel.</param>
        /// <returns>The hotel details if found.</returns>
        /// <response code="200">Returns the hotel details.</response>
        /// <response code="404">If the hotel is not found.</response>
        [HttpGet("{id}", Name = "GetHotelById")]
        [ProducesResponseType(typeof(HotelDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetHotelById(int id)
        {

            var query = new GetHotelByIdQuery { HotelID = id };
            var hotel = await _mediator.Send(query);

            if (hotel == null)
            {
                return NotFound(new { Message = $"Hotel with ID {id} not found." });
            }

            return Ok(hotel);
        }

        /// <summary>
        /// Creates a new hotel with the provided details.
        /// </summary>
        /// <param name="createHotelDto">The details of the hotel to create.</param>
        /// <returns>The created hotel's information.</returns>
        /// <response code="201">Returns the newly created hotel.</response>
        /// <response code="400">If the request body is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(HotelDto), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateHotel(CreateHotelDto createHotelDto)
        {

            var command = new CreateHotelCommand { CreateHotelDto = createHotelDto };
            var hotelid = await _mediator.Send(command);
            if (hotelid == -1)
                return BadRequest(new { Message = "Invalid City or Owner ID body." });

            return CreatedAtRoute("GetHotelById",
                 new
                 {
                     id = hotelid
                 },
                 createHotelDto);
        }

        /// <summary>
        /// Updates the details of an existing hotel.
        /// </summary>
        /// <param name="updateHotelDto">The updated details of the hotel.</param>
        /// <returns>The updated hotel's information.</returns>
        /// <response code="200">Returns the updated hotel.</response>
        /// <response code="400">If the request body is invalid or ID mismatch occurs.</response>
        /// <response code="404">If the hotel to update is not found.</response>
        [HttpPut]
        [ProducesResponseType(typeof(HotelDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateHotel(UpdateHotelDto updateHotelDto)
        {

            var command = new UpdateHotelCommand { UpdateHotelDto = updateHotelDto };
            var updatedHotelDto = await _mediator.Send(command);
            if (updatedHotelDto == null)
                return NotFound(new
                {
                    Message = $"Hotel or City or Owner  ID not found." +
                    $"See The Log Info"
                });


            return Ok(updatedHotelDto);
        }

        /// <summary>
        /// Deletes a hotel by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the hotel to delete.</param>
        /// <returns>No content if deletion is successful.</returns>
        /// <response code="204">Indicates that the hotel was successfully deleted.</response>
        /// <response code="404">If the hotel to delete is not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteHotel(int id)
        {

            var command = new DeleteHotelCommand { HotelID = id };
            var result = await _mediator.Send(command);
            if (result == 0)
                return NotFound(new { Message = $"Hotel with ID {id} not found." });


            return NoContent();
        }
    }
}
