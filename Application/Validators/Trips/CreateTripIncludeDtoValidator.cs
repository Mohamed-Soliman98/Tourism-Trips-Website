using Application.DTOs.Trips;
using FluentValidation;

namespace Application.Validators.Trips
{
    public class CreateTripIncludeDtoValidator : AbstractValidator<CreateTripIncludeDto>
    {
        public CreateTripIncludeDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotNull().WithMessage("Include description is required.");

            RuleFor(x => x.Description.English)
                .NotEmpty().WithMessage("Include English description is required.")
                .MaximumLength(500).WithMessage("Include English description cannot exceed 500 characters.");

            RuleFor(x => x.Description.German)
                .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Description?.German))
                .WithMessage("Include German description cannot exceed 500 characters.");
        }
    }
}
