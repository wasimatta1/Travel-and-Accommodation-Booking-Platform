using Application.Mediator.Commands.HotelCommands;
using FluentValidation;

namespace Application.Validations.HotelVaildator
{
    public class CreateHotelCommandValidator : AbstractValidator<CreateHotelCommand>
    {
        public CreateHotelCommandValidator()
        {

            RuleFor(x => x.CreateHotelDto)
                .NotNull().WithMessage("Hotel details must be provided.");

            RuleFor(x => x.CreateHotelDto.OwnerID)
             .NotEmpty().WithMessage("Owner ID is required.");

            RuleFor(x => x.CreateHotelDto.CityID)
                .NotEmpty().WithMessage("City ID is required.");

            RuleFor(x => x.CreateHotelDto.Name)
                .NotEmpty().WithMessage("Hotel name is required.")
                .MaximumLength(50).WithMessage("Hotel name cannot exceed 50 characters.");

            RuleFor(x => x.CreateHotelDto.LocationGoogelMap)
               .Must(BeAValidUrl).WithMessage("Location GoogelMap URL must be a valid URL.");


            RuleFor(x => x.CreateHotelDto.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(255).WithMessage("Description cannot exceed 255 characters.");

            RuleFor(x => x.CreateHotelDto.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(255).WithMessage("Address cannot exceed 255 characters.");

            RuleFor(x => x.CreateHotelDto.ThumbnailURL)
                .NotEmpty().WithMessage("Thumbnail URL is required.")
                .MaximumLength(255).WithMessage("Thumbnail URL cannot exceed 255 characters.")
                .Must(BeAValidUrl).WithMessage("Thumbnail URL must be a valid URL.");

            RuleFor(x => x.CreateHotelDto.ImageURL)
                .NotEmpty().WithMessage("Image URL is required.")
                .MaximumLength(255).WithMessage("Image URL cannot exceed 255 characters.")
                .Must(BeAValidUrl).WithMessage("Image URL must be a valid URL.");
        }

        private bool BeAValidUrl(string url)
        {
            return Uri.IsWellFormedUriString(url, UriKind.Absolute);
        }
    }


}
