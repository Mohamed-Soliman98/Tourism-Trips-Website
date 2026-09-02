using Application.DTOs.Trips;
using FluentValidation;

namespace Application.Validators.Trips
{
    public class CreateTripFAQDtoValidator : AbstractValidator<CreateTripFAQDto>
    {
        public CreateTripFAQDtoValidator()
        {
            RuleFor(x => x.Question)
                .NotEmpty().WithMessage("FAQ default question is required.")
                .MaximumLength(500).WithMessage("FAQ default question cannot exceed 500 characters.");

            RuleFor(x => x.Answer)
                .NotEmpty().WithMessage("FAQ default answer is required.")
                .MaximumLength(2000).WithMessage("FAQ default answer cannot exceed 2000 characters.");

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Display order must be greater than or equal to 0.");

            RuleFor(x => x.Translations)
                .Must(translations =>
                {
                    if (translations == null || translations.Count == 0) return true;
                    var languages = translations.Select(t => t.Language).ToList();
                    return languages.Count == languages.Distinct().Count();
                })
                .WithMessage("Duplicate translation languages are not allowed for the same FAQ.");

            RuleForEach(x => x.Translations).SetValidator(new CreateFAQTranslationDtoValidator());
        }
    }

    public class CreateFAQTranslationDtoValidator : AbstractValidator<CreateFAQTranslationDto>
    {
        public CreateFAQTranslationDtoValidator()
        {
            RuleFor(x => x.Language)
                .IsInEnum().WithMessage("Invalid language for FAQ translation.");

            RuleFor(x => x.Question)
                .NotEmpty().WithMessage("FAQ question is required.")
                .MaximumLength(500).WithMessage("FAQ question cannot exceed 500 characters.");

            RuleFor(x => x.Answer)
                .NotEmpty().WithMessage("FAQ answer is required.")
                .MaximumLength(2000).WithMessage("FAQ answer cannot exceed 2000 characters.");
        }
    }
}
