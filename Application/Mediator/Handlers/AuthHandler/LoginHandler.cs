using Application.Mediator.Commands.AuthCommands;
using Contracts.DTOs.Authentication;
using Contracts.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.AuthHandler
{
    public class LoginHandler : IRequestHandler<LoginCommand, AuthResponseDto>
    {
        private readonly UserManager<User> _userManger;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;
        private readonly ILogger<LoginHandler> _logger;


        public LoginHandler(UserManager<User> user, ITokenService tokenService, SignInManager<User> signInManager, ILogger<LoginHandler> logger)
        {
            _userManger = user;
            _tokenService = tokenService;
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<AuthResponseDto> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Handling LoginCommand for User: {request.LoginRequest.Email}");
            var user = await _userManger.FindByEmailAsync(request.LoginRequest.Email);
            if (user == null)
            {
                _logger.LogWarning($"User with email: {request.LoginRequest.Email} was not found");
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.LoginRequest.Password, false);
            if (!result.Succeeded)
            {
                _logger.LogWarning($"Invalid password for user: {request.LoginRequest.Email}");
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "Invalid password"
                };
            }

            var role = await _userManger.GetRolesAsync(user);

            return new AuthResponseDto
            {
                IsSuccess = true,
                Token = await _tokenService.GenerateTokenAsync(user.Email, role[0])
            };
        }
    }

}
