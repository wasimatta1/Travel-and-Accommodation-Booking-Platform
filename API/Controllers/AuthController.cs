
using Application.Mediator.Commands;
using Contracts.Authentication;
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
            _mediator = mediator;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="registerRequest"></param>
        /// <returns></returns>

        [HttpPost("register")]
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
        [HttpPost("login")]
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

        [HttpPost("Logout")]
        [Authorize]
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
