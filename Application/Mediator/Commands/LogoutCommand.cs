using Contracts.Authentication;
using MediatR;

namespace Application.Mediator.Commands
{
    public class LogoutCommand : IRequest<AuthResponse>
    {
        public string Token { get; set; } 
    }

}
