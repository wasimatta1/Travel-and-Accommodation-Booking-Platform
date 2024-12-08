﻿using Application.Mediator.Commands;
using FluentValidation;

namespace Application.Validations
{
    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.LoginRequest.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(x => x.LoginRequest.Password)
                .NotEmpty().WithMessage("Password is required.");
        }
    }

}
