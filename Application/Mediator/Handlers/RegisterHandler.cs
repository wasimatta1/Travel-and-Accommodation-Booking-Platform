using Application.Mediator.Commands;
using AutoMapper;
using Contracts.Authentication;
using Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Application.Mediator.Handlers
{
    public class RegisterHandler : IRequestHandler<RegisterCommand, AuthResponse>
    {
        private readonly UserManager<User> _user;
        private readonly IMapper _mapper;

        public RegisterHandler(UserManager<User> user, IMapper mapper)
        {
            _user = user;
            _mapper = mapper;
        }

        public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var user = _mapper.Map<User>(request.RegisteredUser);
            user.UserName = user.Email;


            var result = await _user.CreateAsync(user, request.RegisteredUser.Password);

            if (!result.Succeeded)
            {
                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "User registration failed",
                    Errors = result.Errors.Select(e => e.Description)

                };
            }

            var roleResult = await _user.AddToRoleAsync(user, "User");

            if (!roleResult.Succeeded)
            {
                await _user.DeleteAsync(user);

                return new AuthResponse
                {
                    IsSuccess = false,
                    Message = "User registration successful, but role assignment failed. User has been removed.",
                    Errors = result.Errors.Select(e => e.Description)
                };
            }

            return new AuthResponse
            {
                IsSuccess = true,
                Message = "User registered and assigned to role successfully"
            };

        }

    }
}
