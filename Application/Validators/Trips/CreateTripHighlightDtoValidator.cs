using Application.DTOs.Trips;
using FluentValidation;

namespace Application.Validators.Trips
{
    public class CreateTripHighlightDtoValidator : AbstractValidator<CreateTripHighlightDto>
    {
        public CreateTripHighlightDtoValidator()
        {
            RuleFor(x => x.Description)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("Highlight description is required.")
                .MaximumLength(500).WithMessage("Highlight description cannot exceed 500 characters.");

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Display order must be greater than or equal to 0.");
        }
    }
}
