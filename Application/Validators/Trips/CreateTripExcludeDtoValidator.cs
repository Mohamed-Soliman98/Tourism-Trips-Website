using Application.DTOs.Trips;
using FluentValidation;

namespace Application.Validators.Trips
{
    public class CreateTripExcludeDtoValidator : AbstractValidator<CreateTripExcludeDto>
    {
        public CreateTripExcludeDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotNull().WithMessage("Exclude description is required.");

            RuleFor(x => x.Description.English)
                .NotEmpty().WithMessage("Exclude English description is required.")
                .MaximumLength(500).WithMessage("Exclude English description cannot exceed 500 characters.");

            RuleFor(x => x.Description.German)
                .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Description?.German))
                .WithMessage("Exclude German description cannot exceed 500 characters.");
        }
    }
}
