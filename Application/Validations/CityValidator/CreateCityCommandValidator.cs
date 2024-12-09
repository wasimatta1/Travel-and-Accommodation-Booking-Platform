using Application.Mediator.Commands.CityCommands;
using FluentValidation;

namespace Application.Validations.CityValidator
{
    public class CreateCityCommandValidator : AbstractValidator<CreateCityCommand>
    {
        public CreateCityCommandValidator()
        {
            RuleFor(x => x.CreateCityDto)
                .NotNull().WithMessage("CreateCityDto cannot be null.");

            RuleFor(x => x.CreateCityDto.Name)
                .NotEmpty().WithMessage("City name is required.")
                .MaximumLength(50).WithMessage("City name cannot exceed 50 characters.");

            RuleFor(x => x.CreateCityDto.Country)
                .NotEmpty().WithMessage("Country is required.")
                .MaximumLength(50).WithMessage("Country cannot exceed 50 characters.");

            RuleFor(x => x.CreateCityDto.ThumbnailURL)
                .NotEmpty().WithMessage("Thumbnail URL is required.")
                .MaximumLength(255).WithMessage("Thumbnail URL cannot exceed 255 characters.")
                .Must(BeAValidUrl).WithMessage("Thumbnail URL must be a valid URL.");

            RuleFor(x => x.CreateCityDto.PostOffice)
                 .NotEmpty().WithMessage("PostOffice  is required.")
                 .MaximumLength(50).WithMessage("Post Office code cannot exceed 50 characters.");

        }

        private bool BeAValidUrl(string url)
        {
            return Uri.IsWellFormedUriString(url, UriKind.Absolute);
        }
    }

}
