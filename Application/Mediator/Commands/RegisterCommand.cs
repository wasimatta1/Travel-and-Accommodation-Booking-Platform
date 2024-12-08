using Contracts.DTOs.Authentication;
using MediatR;

namespace Application.Mediator.Commands
{
    public class RegisterCommand : IRequest<AuthResponse>
    {
        public RegisterRequest RegisteredUser { get; set; }
    }
}
