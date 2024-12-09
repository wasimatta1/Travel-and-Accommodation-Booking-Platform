using Contracts.DTOs.Authentication;
using MediatR;

namespace Application.Mediator.Commands.AuthCommands
{
    public class LoginCommand : IRequest<AuthResponse>
    {
        public LoginRequest LoginRequest { get; set; }
    }

}
