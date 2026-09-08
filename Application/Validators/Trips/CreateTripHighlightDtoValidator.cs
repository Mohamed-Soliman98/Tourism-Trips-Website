using Application.DTOs.Trips;
using FluentValidation;

namespace Application.Validators.Trips
{
    public class CreateTripHighlightDtoValidator : AbstractValidator<CreateTripHighlightDto>
    {
        public CreateTripHighlightDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotNull().WithMessage("Highlight description is required.");

            RuleFor(x => x.Description.English)
                .NotEmpty().WithMessage("Highlight English description is required.")
                .MaximumLength(500).WithMessage("Highlight English description cannot exceed 500 characters.");

            RuleFor(x => x.Description.German)
                .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Description?.German))
                .WithMessage("Highlight German description cannot exceed 500 characters.");

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Display order must be greater than or equal to 0.");
        }
    }
}
