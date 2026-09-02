using Application.DTOs.Trips;
using FluentValidation;

namespace Application.Validators.Trips
{
    public class CreateTripExcludeDtoValidator : AbstractValidator<CreateTripExcludeDto>
    {
        public CreateTripExcludeDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Exclude description is required.")
                .MaximumLength(500).WithMessage("Exclude description cannot exceed 500 characters.");
        }
    }
}
