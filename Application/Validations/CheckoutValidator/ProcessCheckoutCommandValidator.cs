using Application.Mediator.Commands.CheckoutCommands;
using FluentValidation;

namespace Application.Validations.CheckoutValidator
{
    public class ProcessCheckoutCommandValidator : AbstractValidator<ProcessCheckoutCommand>
    {
        public ProcessCheckoutCommandValidator()
        {
            RuleFor(x => x.CheckoutInfo.CardNumber)
                .NotEmpty().WithMessage("Card number is required.")
                .Length(16).WithMessage("Card number must be 16 digits.")
                .Matches(@"^\d{16}$").WithMessage("Card number must be 16 digits.")
                .Must(BeAValidCardNumber).WithMessage("Card number must be Visa, MasterCard or American Express.");
            RuleFor(x => x.CheckoutInfo.CardHolderName)
                .NotEmpty().WithMessage("Card holder name is required.")
                .MaximumLength(50).WithMessage("Card holder name must be less than 50 characters.");
            RuleFor(x => x.CheckoutInfo.ExpiryDate)
                .NotEmpty().WithMessage("Expiry date is required.")
                .Matches(@"^(0[1-9]|1[0-2])\/?([0-9]{2})$").WithMessage("Expiry date must be in the format MM/YY.");
            //.Must(BeAValidExpiryDate).WithMessage("Expiry date must be in the feature.");
            RuleFor(x => x.CheckoutInfo.CVV)
                .NotEmpty().WithMessage("CVV is required.")
                .Length(3).WithMessage("CVV must be 3 digits.");
            RuleFor(x => x.CheckoutInfo.SpecialRequests)
                .MaximumLength(500).WithMessage("Special requests must be less than 500 characters.");
        }
        private bool BeAValidExpiryDate(string expiryDate)
        {
            return DateOnly.TryParse(expiryDate, out var date) && date > DateOnly.FromDateTime(DateTime.Now);
        }
        private bool BeAValidCardNumber(string cardNumber)
        {
            return cardNumber.StartsWith("100") || cardNumber.StartsWith("101") || cardNumber.StartsWith("102");
        }
    }
}
