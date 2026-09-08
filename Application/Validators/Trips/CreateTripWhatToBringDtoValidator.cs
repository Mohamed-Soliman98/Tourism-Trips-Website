using Application.DTOs.Trips;
using FluentValidation;

namespace Application.Validators.Trips
{
    public class CreateTripWhatToBringDtoValidator : AbstractValidator<CreateTripWhatToBringDto>
    {
        public CreateTripWhatToBringDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotNull().WithMessage("What to bring description is required.");

            RuleFor(x => x.Description.English)
                .NotEmpty().WithMessage("What to bring English description is required.")
                .MaximumLength(500).WithMessage("What to bring English description cannot exceed 500 characters.");

            RuleFor(x => x.Description.German)
                .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Description?.German))
                .WithMessage("What to bring German description cannot exceed 500 characters.");

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Display order must be greater than or equal to 0.");
        }
    }
}
