﻿using Application.Mediator.Commands.AuthCommands;
using Contracts.DTOs.Authentication;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Application.Mediator.Handlers.AuthHandler
{
    public class LogoutHandler : IRequestHandler<LogoutCommand, AuthResponse>
    {
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<LogoutHandler> _logger;

        public LogoutHandler(SignInManager<User> signInManager, ILogger<LogoutHandler> logger)
        {
            _signInManager = signInManager;
            _logger = logger;
        }

        public async Task<AuthResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Handling LogoutCommand");
            await _signInManager.SignOutAsync();

            return new AuthResponse
            {
                IsSuccess = true,
                Message = "User logged out successfully"
            };
        }
    }
}