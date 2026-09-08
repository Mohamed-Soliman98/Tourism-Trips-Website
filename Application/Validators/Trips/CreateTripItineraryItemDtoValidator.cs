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
                .NotNull().WithMessage("Itinerary item title is required.");

            When(x => x.Title != null, () =>
            {
                RuleFor(x => x.Title.English)
                    .NotEmpty().WithMessage("Itinerary item English title is required.")
                    .MaximumLength(200).WithMessage("Itinerary item English title cannot exceed 200 characters.");

                RuleFor(x => x.Title.German)
                    .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Title.German))
                    .WithMessage("Itinerary item German title cannot exceed 200 characters.");
            });

            When(x => x.Description != null, () =>
            {
                RuleFor(x => x.Description!.English)
                    .MaximumLength(1000).When(x => !string.IsNullOrEmpty(x.Description?.English))
                    .WithMessage("Itinerary item English description cannot exceed 1000 characters.");

                RuleFor(x => x.Description!.German)
                    .MaximumLength(1000).When(x => !string.IsNullOrEmpty(x.Description?.German))
                    .WithMessage("Itinerary item German description cannot exceed 1000 characters.");
            });
        }
    }
}
