using Application.Mediator.Commands.HotelPageCommands;
using FluentValidation;

namespace Application.Validations.CartVaildations
{
    public class AddToCartCommandValidator : AbstractValidator<AddToCartCommand>
    {
        public AddToCartCommandValidator()
        {
            RuleFor(x => x.CartItem).NotNull();
        }
    }

}
