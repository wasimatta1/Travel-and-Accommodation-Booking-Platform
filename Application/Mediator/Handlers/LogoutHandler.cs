using Application.Mediator.Commands;
using Contracts.Authentication;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Mediator.Handlers
{
    public class LogoutHandler : IRequestHandler<LogoutCommand, AuthResponse>
    {
        private readonly SignInManager<User> _signInManager;

        public LogoutHandler(SignInManager<User> signInManager)
        {
            _signInManager = signInManager;
        }

        public async Task<AuthResponse> Handle(LogoutCommand request, CancellationToken cancellationToken)
        {
            await _signInManager.SignOutAsync();




            return new AuthResponse
            {
                IsSuccess = true,
                Message = "User logged out successfully"
            };
        }
    }
}
