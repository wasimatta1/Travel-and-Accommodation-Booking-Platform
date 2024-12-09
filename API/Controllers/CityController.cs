using Application.Mediator.Commands.CityCommands;
using Application.Mediator.Queries.CityQueries;
using Contracts.DTOs.City;
using MediatR;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CityController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }


        /// <summary>
        /// Retrieves a paginated list of cities with optional filtering by city name, country, and post office code.
        /// </summary>
        /// <param name="pagNumber">The page number for pagination.</param>
        /// <param name="pageSize">The number of records per page.</param>
        /// <param name="CityName">Optional filter to search cities by name.</param>
        /// <param name="Country">Optional filter to search cities by country.</param>
        /// <param name="PostOffice">Optional filter to search cities by post office code.</param>
        /// <returns>A paginated list of cities matching the specified filters.</returns>
        /// <response code="200">Returns the list of cities.</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<CityDto>), 200)]
        public async Task<IActionResult> GetAllAsync(int pagNumber = 1, int pageSize = 10,
            string CityName = null, string Country = null, string PostOffice = null)
        {

            var query = new GetAllCitiesQuery
            {
                CityName = CityName,
                Country = Country,
                PostOffice = PostOffice,
                PagNumber = pagNumber,
                PageSize = pageSize,

            };
            return Ok(await _mediator.Send(query));
        }


        /// <summary>
        /// Retrieves detailed information about a specific city by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the city.</param>
        /// <returns>The city details if found.</returns>
        /// <response code="200">Returns the city details.</response>
        /// <response code="404">If the city is not found.</response>
        [HttpGet("{id}", Name = "GetCityById")]
        [ProducesResponseType(typeof(CityDto), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCityById(int id)
        {

            var query = new GetCityByIdQuery { CityID = id };
            var city = await _mediator.Send(query);
            if (city == null)
                return NotFound(new { Message = $"City with ID {id} not found." });

            return Ok(city);
        }

        /// <summary>
        /// Creates a new city with the provided details.
        /// </summary>
        /// <param name="createCityDto">The details of the city to create.</param>
        /// <returns>The created city's information.</returns>
        /// <response code="201">Returns the newly created city.</response>
        /// <response code="400">If the request body is invalid.</response>
        [HttpPost]
        [ProducesResponseType(typeof(CityDto), 201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCity(CreateCityDto createCityDto)
        {
            var command = new CreateCityCommand { CreateCityDto = createCityDto };
            var cityId = await _mediator.Send(command);

            return CreatedAtRoute("GetCityById",
                 new
                 {
                     id = cityId
                 },
                 createCityDto);
        }

        /// <summary>
        /// Updates the details of an existing city.
        /// </summary>
        /// <param name="updateCityDto">The updated details of the city.</param>
        /// <returns>The updated city's information.</returns>
        /// <response code="200">Returns the updated city.</response>
        /// <response code="400">If the request body is invalid.</response>
        /// <response code="404">If the city to update is not found.</response>
        [HttpPut]
        [ProducesResponseType(typeof(CityDto), 200)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCity(UpdateCityDto updateCityDto)
        {
            var command = new UpdateCityCommand { UpdateCityDto = updateCityDto };
            var city = await _mediator.Send(command);
            if (city == null)
                return NotFound(new { Message = $"City with ID {updateCityDto.CityID} not found." });

            return Ok(city);
        }

        /// <summary>
        /// Deletes a city by its unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the city to delete.</param>
        /// <returns>No content if deletion is successful.</returns>
        /// <response code="204">Indicates that the city was successfully deleted.</response>
        /// <response code="404">If the city to delete is not found.</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCity(int id)
        {
            var command = new DeleteCityCommand { CityID = id };
            var change = await _mediator.Send(command);
            if (change == 0)
                return NotFound(new { Message = $"City with ID {id} not found." });

            return NoContent();
        }

    }
}
