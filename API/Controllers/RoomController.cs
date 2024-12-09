using Application.Mediator.Commands.RoomCommands;
using Application.Mediator.Queries.RoomQueries;
using Contracts.DTOs.Room;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RoomController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

        }

        /// <summary>
        /// Retrieves a paginated list of rooms with optional filtering by room number, type, adult capacity, children capacity, and availability.
        /// </summary>
        /// <param name="pagNumber">The page number for pagination .</param>
        /// <param name="pageSize">The number of records per page .</param>
        /// <param name="RoomNumber">Optional filter to search rooms by room number.</param>
        /// <param name="Type">Optional filter to search rooms by type.</param>
        /// <param name="AdultCapacity">Optional filter to search rooms by adult capacity.</param>
        /// <param name="ChildrenCapacity">Optional filter to search rooms by children capacity.</param>
        /// <param name="Availability">Optional filter to search rooms by availability status.</param>
        /// <returns>A paginated list of rooms matching the specified filters.</returns>
        /// <response code="200">Returns the list of rooms.</response>
        /// <response code="400">If the request parameters are invalid.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<RoomDto>), 200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllAsync(int pagNumber = 1, int pageSize = 10,
            string? RoomNumber = null, string? Type = null, int? AdultCapacity = null, int? ChildrenCapacity = null, bool? Availability = null)
        {

            var query = new GetAllRoomsQuery
            {
                RoomNumber = RoomNumber,
                Type = Type,
                AdultCapacity = AdultCapacity,
                ChildrenCapacity = ChildrenCapacity,
                Availability = Availability,
                PagNumber = pagNumber,
                PageSize = pageSize,
            };

            var result = await _mediator.Send(query);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves detailed information about a specific room by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the room.</param>
        /// <returns>The room details if found.</returns>
        /// <response code="200">Returns the room details.</response>
        /// <response code="404">If the room is not found.</response>
        [HttpGet("{id}", Name = "GetRoomById")]
        [ProducesResponseType(typeof(RoomDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetRoomById(int id)
        {

            var query = new GetRoomByIdQuery { RoomID = id };
            var room = await _mediator.Send(query);

            if (room == null)
                return NotFound(new { Message = $"Room with ID {id} not found." });


            return Ok(room);
        }

        /// <summary>
        /// Creates a new room with the provided details.
        /// </summary>
        /// <param name="createRoomDto">The details of the room to create.</param>
        /// <returns>The created room's information.</returns>
        /// <response code="201">Returns the newly created room.</response>
        /// <response code="400">If the request body is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(RoomDto), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateRoom(CreateRoomDto createRoomDto)
        {

            var command = new CreateRoomCommand { CreateRoomDto = createRoomDto };
            var roomId = await _mediator.Send(command);

            if (roomId == -1)
                return BadRequest(new { Message = "Invalid Hotel ID provided." });


            return CreatedAtRoute("GetRoomById",
                 new
                 {
                     id = roomId
                 },
                 createRoomDto);
        }

        /// <summary>
        /// Updates the details of an existing room.
        /// </summary>
        /// <param name="updateRoomDto">The updated details of the room.</param>
        /// <returns>The updated room's information.</returns>
        /// <response code="200">Returns the updated room.</response>
        /// <response code="400">If the request body is invalid.</response>
        /// <response code="404">If the room to update is not found.</response>
        [HttpPut]
        [ProducesResponseType(typeof(RoomDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateRoom(UpdateRoomDto updateRoomDto)
        {

            var command = new UpdateRoomCommand { UpdateRoomDto = updateRoomDto };
            var updatedRoomDto = await _mediator.Send(command);

            if (updatedRoomDto == null)
                return NotFound(new
                {
                    Message = $"Room or Hotel ID not found."
                });


            return Ok(updatedRoomDto);
        }

        /// <summary>
        /// Deletes a room by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the room to delete.</param>
        /// <returns>No content if deletion is successful.</returns>
        /// <response code="204">Indicates that the room was successfully deleted.</response>
        /// <response code="404">If the room to delete is not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var command = new DeleteRoomCommand { RoomID = id };
            var result = await _mediator.Send(command);

            if (result == 0)
                return NotFound(new { Message = $"Room with ID {id} not found." });


            return NoContent();
        }
    }
}
