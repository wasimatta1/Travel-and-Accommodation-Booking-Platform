using Application.Mediator.Commands.HotelCommands;
using FluentValidation;

namespace Application.Validations.HotelVaildator
{
    public class UpdateHotelCommandValidator : AbstractValidator<UpdateHotelCommand>
    {
        public UpdateHotelCommandValidator()
        {
            RuleFor(x => x.UpdateHotelDto)
                .NotNull().WithMessage("Hotel details must be provided for update.");

            RuleFor(x => x.UpdateHotelDto.HotelID)
                .NotEmpty().WithMessage("Hotel ID is required.")
                .GreaterThan(0).WithMessage("Hotel ID must be a positive integer.");

            RuleFor(x => x.UpdateHotelDto.OwnerID)
                .NotEmpty().WithMessage("Owner ID is required.");

            RuleFor(x => x.UpdateHotelDto.CityID)
                .NotEmpty().WithMessage("City ID is required.");

            RuleFor(x => x.UpdateHotelDto.Name)
                .NotEmpty().WithMessage("Hotel name is required.")
                .MaximumLength(50).WithMessage("Hotel name cannot exceed 50 characters.");


            RuleFor(x => x.UpdateHotelDto.Description)
               .NotEmpty().WithMessage("Description is required.")
               .MaximumLength(255).WithMessage("Description cannot exceed 255 characters.");

            RuleFor(x => x.UpdateHotelDto.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(255).WithMessage("Address cannot exceed 255 characters.");

            RuleFor(x => x.UpdateHotelDto.LocationGoogelMap)
               .Must(BeAValidUrl).WithMessage("Location GoogelMap URL must be a valid URL.");

            RuleFor(x => x.UpdateHotelDto.ThumbnailURL)
                .NotEmpty().WithMessage("Thumbnail URL is required.")
                .MaximumLength(255).WithMessage("Thumbnail URL cannot exceed 255 characters.")
                .Must(BeAValidUrl).WithMessage("Thumbnail URL must be a valid URL.");

            RuleFor(x => x.UpdateHotelDto.ImageURL)
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
