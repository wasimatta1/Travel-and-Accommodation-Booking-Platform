using Application.Mediator.Commands.AuthCommands;
using AutoMapper;
using Contracts.DTOs.Authentication;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.AuthHandler
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, AuthResponseDto>
    {
        private readonly UserManager<User> _user;
        private readonly IMapper _mapper;
        private readonly ILogger<RegisterHandler> _logger;

        public RegisterHandler(UserManager<User> user, IMapper mapper, ILogger<RegisterHandler> logger)
        {
            _user = user;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<AuthResponseDto> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {

            _logger.LogInformation($"Handling RegisterCommand for User: {request.RegisteredUser.Email}");

            var user = _mapper.Map<User>(request.RegisteredUser);
            user.UserName = user.Email;


            var result = await _user.CreateAsync(user, request.RegisteredUser.Password);

            if (!result.Succeeded)
            {
                _logger.LogWarning($"User registration failed for user: {request.RegisteredUser.Email}");
                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User registration failed",
                    Errors = result.Errors.Select(e => e.Description)

                };
            }

            var roleResult = await _user.AddToRoleAsync(user, "User");

            if (!roleResult.Succeeded)
            {
                _logger.LogWarning($"Role assignment failed for user: {request.RegisteredUser.Email}");
                await _user.DeleteAsync(user);

                return new AuthResponseDto
                {
                    IsSuccess = false,
                    Message = "User registration successful, but role assignment failed. User has been removed.",
                    Errors = result.Errors.Select(e => e.Description)
                };
            }

            _logger.LogInformation($"User: {request.RegisteredUser.Email} registered and assigned to role successfully");
            return new AuthResponseDto
            {
                IsSuccess = true,
                Message = "User registered and assigned to role successfully"
            };

        }

    }
}
