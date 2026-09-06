using Application.DTOs.Trips;
using FluentValidation;

namespace Application.Validators.Trips
{
    public class CreateTripWhatToBringDtoValidator : AbstractValidator<CreateTripWhatToBringDto>
    {
        public CreateTripWhatToBringDtoValidator()
        {
            RuleFor(x => x.Description)
                .Must(x => !string.IsNullOrWhiteSpace(x)).WithMessage("What to bring description is required.")
                .MaximumLength(500).WithMessage("What to bring description cannot exceed 500 characters.");

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Display order must be greater than or equal to 0.");
        }
    }
}
