using Contracts.DTOs.Authentication;
using MediatR;

namespace Application.Mediator.Commands.AuthCommands
{
    public class RegisterCommand : IRequest<AuthResponseDto>
    {
        public RegisterRequestDto RegisteredUser { get; set; }
    }
}
