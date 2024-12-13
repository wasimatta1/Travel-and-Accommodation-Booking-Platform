using Contracts.DTOs.Authentication;
using MediatR;

namespace Application.Mediator.Commands.AuthCommands
{
    public class LoginCommand : IRequest<AuthResponseDto>
    {
        public LoginRequestDto LoginRequest { get; set; }
    }

}
