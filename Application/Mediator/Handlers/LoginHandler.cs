using Application.Mediator.Commands;
using Contracts.Authentication;
using Contracts.Interfaces;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Mediator.Handlers
{
    public class LoginHandler : IRequestHandler<LoginCommand, AuthResponse>
    {
        private readonly UserManager<User> _userManger;
        private readonly SignInManager<User> _signInManager;
        private readonly ITokenService _tokenService;

        public LoginHandler(UserManager<User> user, ITokenService tokenService, SignInManager<User> signInManager)
        {
            _userManger = user;
            _tokenService = tokenService;
            _signInManager = signInManager;
        }

        public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManger.FindByEmailAsync(request.LoginRequest.Email);
            if (user == null)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "User not found"
                };
            }
            var result = await _signInManager.CheckPasswordSignInAsync(user, request.LoginRequest.Password, false);
            if (!result.Succeeded)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "Invalid password"
                };
            }

            var role = await _userManger.GetRolesAsync(user);

            return new AuthResponse
            {
                IsSuccess = true,
                Token = await _tokenService.GenerateTokenAsync(user.Email, role[0])
            };
        }
    }

}
