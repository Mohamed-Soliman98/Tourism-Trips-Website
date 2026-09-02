using Application.DTOs.Trips;
using FluentValidation;

namespace Application.Validators.Trips
{
    public class CreateTripItineraryItemDtoValidator : AbstractValidator<CreateTripItineraryItemDto>
    {
        public CreateTripItineraryItemDtoValidator()
        {
            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Display order must be greater than or equal to 0.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Itinerary item title is required.")
                .MaximumLength(200).WithMessage("Itinerary item title cannot exceed 200 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Itinerary item description cannot exceed 1000 characters.");
        }
    }
}
