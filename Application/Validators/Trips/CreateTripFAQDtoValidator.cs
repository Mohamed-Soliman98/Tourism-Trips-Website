using Application.DTOs.Trips;
using FluentValidation;

namespace Application.Validators.Trips
{
    public class CreateTripFAQDtoValidator : AbstractValidator<CreateTripFAQDto>
    {
        public CreateTripFAQDtoValidator()
        {
            RuleFor(x => x.Question)
                .NotNull().WithMessage("FAQ question is required.");

            RuleFor(x => x.Question.English)
                .NotEmpty().WithMessage("FAQ English question is required.")
                .MaximumLength(500).WithMessage("FAQ English question cannot exceed 500 characters.");

            RuleFor(x => x.Question.German)
                .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Question?.German))
                .WithMessage("FAQ German question cannot exceed 500 characters.");

            RuleFor(x => x.Answer)
                .NotNull().WithMessage("FAQ answer is required.");

            RuleFor(x => x.Answer.English)
                .NotEmpty().WithMessage("FAQ English answer is required.")
                .MaximumLength(2000).WithMessage("FAQ English answer cannot exceed 2000 characters.");

            RuleFor(x => x.Answer.German)
                .MaximumLength(2000).When(x => !string.IsNullOrEmpty(x.Answer?.German))
                .WithMessage("FAQ German answer cannot exceed 2000 characters.");

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Display order must be greater than or equal to 0.");
        }
    }
}
