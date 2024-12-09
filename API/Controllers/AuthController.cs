using Application.Mediator.Commands.AuthCommands;
using Contracts.DTOs.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;

        public AuthController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        /// <summary>
        /// Registers a new user with the provided registration details.
        /// </summary>
        /// <param name="registerRequest">The registration details of the user.</param>
        /// <returns>A success message if registration is successful; otherwise, returns validation errors.</returns>
        /// <response code="200">Returns a success message upon successful registration.</response>
        /// <response code="400">Returns validation errors if the registration details are invalid.</response>
        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register(RegisterRequest registerRequest)
        {
            var command = new RegisterCommand { RegisteredUser = registerRequest };
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
                return Ok(response.Message);

            return BadRequest(new
            {
                response.Message,
                response.Errors
            });
        }


        /// <summary>
        /// Authenticates a user with the provided login credentials.
        /// </summary>
        /// <param name="loginRequest">The login credentials of the user.</param>
        /// <returns>A JWT token if authentication is successful; otherwise, returns an error message.</returns>
        /// <response code="200">Returns a JWT token upon successful authentication.</response>
        /// <response code="400">Returns an error message if authentication fails.</response>
        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Login(LoginRequest loginRequest)
        {
            var command = new LoginCommand { LoginRequest = loginRequest };
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
                return Ok(new
                {
                    Token = response.Token
                });

            return BadRequest(response.Message);
        }

        /// <summary>
        /// Logs out the currently authenticated user.
        /// </summary>
        /// <returns>A success message if logout is successful; otherwise, returns an error message.</returns>
        /// <response code="200">Returns a success message upon successful logout.</response>
        /// <response code="400">Returns an error message if logout fails.</response>
        [HttpPost("logout")]
        [Authorize]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Logout()
        {
            var command = new LogoutCommand();
            var response = await _mediator.Send(command);
            if (response.IsSuccess)
                return Ok(response.Message);

            return BadRequest(response.Message);
        }

    }
}
