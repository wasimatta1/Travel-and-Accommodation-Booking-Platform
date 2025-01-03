using Application.Mediator.Commands.AuthCommands;
using Contracts.DTOs.Authentication;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.AuthHandler
{
    public class LogoutHandler : IRequestHandler<LogoutCommand, AuthResponseDto>
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LogoutHandler> _logger;
        private readonly IHttpContextAccessor _contextAccessor;

        public LogoutHandler(SignInManager<User> signInManager, ILogger<LogoutHandler> logger, IHttpContextAccessor contextAccessor)
        {
            _signInManager = signInManager;
            _contextAccessor = contextAccessor;
            _logger = logger;
        }

        public async Task<AuthResponseDto> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling LogoutCommand");
            await _signInManager.SignOutAsync();
            await _contextAccessor.HttpContext!.SignOutAsync();

            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "User logged out successfully"
            };
        }
    }
}
