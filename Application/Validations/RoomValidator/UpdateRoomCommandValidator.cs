using Application.Mediator.Commands.RoomCommands;
using Domain.Enums;
using FluentValidation;

namespace Application.Validations.RoomValidator
{
    public class UpdateRoomCommandValidator : AbstractValidator<UpdateRoomCommand>
    {
        public UpdateRoomCommandValidator()
        {
            RuleFor(x => x.UpdateRoomDto.RoomID)
                .NotEmpty().WithMessage("Room ID is required.")
                .GreaterThan(0).WithMessage("Room ID must be greater than 0.");

            RuleFor(x => x.UpdateRoomDto.RoomNumber)
                .NotEmpty().WithMessage("Room number is required.")
                .MaximumLength(10).WithMessage("Room number must not exceed 10 characters.");

            RuleFor(x => x.UpdateRoomDto.RoomType)
                .NotNull().WithMessage("Room type is required.")
                .IsEnumName(typeof(RoomType)).WithMessage("Invalid room type.");

            RuleFor(x => x.UpdateRoomDto.AdultsCapacity)
                .GreaterThan(0).WithMessage("Adult capacity must be greater than 0.");

            RuleFor(x => x.UpdateRoomDto.ChildrenCapacity)
                .GreaterThanOrEqualTo(0).WithMessage("Children capacity must be greater than or equal to 0.");

            RuleFor(x => x.UpdateRoomDto.PricePerNight)
                .GreaterThan(0).WithMessage("Price per night must be greater than 0.");

            RuleFor(x => x.UpdateRoomDto.Description)
                .MaximumLength(255).WithMessage("Description must not exceed 255 characters.");

            RuleFor(x => x.UpdateRoomDto.ImagesUrl)
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
