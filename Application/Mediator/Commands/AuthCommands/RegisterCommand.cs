using Contracts.DTOs.Authentication;
using MediatR;

namespace Application.Mediator.Commands.AuthCommands
{
    public class RegisterCommand : IRequest<AuthResponse>
    {
        public RegisterRequest RegisteredUser { get; set; }
    }
}
