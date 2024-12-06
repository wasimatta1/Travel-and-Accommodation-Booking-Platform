using Contracts.Authentication;
using MediatR;

namespace Application.Mediator.Commands
{
    public class LoginCommand : IRequest<AuthResponse>
    {
        public LoginRequest LoginRequest { get; set; }
    }
}
