using Application.DTOs.Trips;
using FluentValidation;

namespace Application.Validators.Trips
{
    public class CreateTripIncludeDtoValidator : AbstractValidator<CreateTripIncludeDto>
    {
        public CreateTripIncludeDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Include description is required.")
                .MaximumLength(500).WithMessage("Include description cannot exceed 500 characters.");
        }
    }
}
