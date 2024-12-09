using Application.Mediator.Commands.RoomCommands;
using Domain.Enums;
using FluentValidation;

namespace Application.Validations.RoomValidator
{
    public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
    {
        public CreateRoomCommandValidator()
        {
            RuleFor(x => x.CreateRoomDto.RoomNumber)
                .NotEmpty().WithMessage("Room number is required.")
                .MaximumLength(10).WithMessage("Room number must not exceed 10 characters.");

            RuleFor(x => x.CreateRoomDto.HotelID)
                .NotEmpty().WithMessage("Hotel ID is required.")
                .GreaterThan(0).WithMessage("Hotel ID must be greater than 0.");

            RuleFor(x => x.CreateRoomDto.RoomType)
                .NotNull().WithMessage("Room type is required.")
                .IsEnumName(typeof(RoomType)).WithMessage("Invalid room type.");


            RuleFor(x => x.CreateRoomDto.AdultsCapacity)
                .GreaterThan(0).WithMessage("Adult capacity must be greater than 0.");

            RuleFor(x => x.CreateRoomDto.ChildrenCapacity)
                .GreaterThanOrEqualTo(0).WithMessage("Children capacity must be greater than or equal to 0.");

            RuleFor(x => x.CreateRoomDto.PricePerNight)
                .GreaterThan(0).WithMessage("Price per night must be greater than 0.");

            RuleFor(x => x.CreateRoomDto.Description)
                .MaximumLength(255).WithMessage("Description must not exceed 255 characters.");

            RuleFor(x => x.CreateRoomDto.ImagesUrl)
                .Must(BeAValidUrl).WithMessage("Room images must be valid URLs.");

        }

        private bool BeAValidUrl(IEnumerable<string> enumerable)
        {
            foreach (var url in enumerable)
            {
                if (!Uri.IsWellFormedUriString(url, UriKind.Absolute))
                {
                    return false;
                }
            }
            return true;

        }

    }
}
