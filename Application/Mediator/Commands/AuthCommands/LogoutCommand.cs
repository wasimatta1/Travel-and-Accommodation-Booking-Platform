using Contracts.DTOs.Authentication;
using MediatR;

namespace Application.Mediator.Commands.AuthCommands
{
    public class LogoutCommand : IRequest<AuthResponse>
    {
        public string Token { get; set; }
    }

}
