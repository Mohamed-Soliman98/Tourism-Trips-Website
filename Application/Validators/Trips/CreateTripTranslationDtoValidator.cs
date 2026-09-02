using Application.DTOs.Trips;
using FluentValidation;

namespace Application.Validators.Trips
{
    public class CreateTripTranslationDtoValidator : AbstractValidator<CreateTripTranslationDto>
    {
        public CreateTripTranslationDtoValidator()
        {
            RuleFor(x => x.Language)
                .IsInEnum().WithMessage("Invalid language specified for translation.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Translation title is required.")
                .MaximumLength(200).WithMessage("Translation title cannot exceed 200 characters.");

            RuleFor(x => x.ShortDescription)
                .NotEmpty().WithMessage("Translation short description is required.")
                .MaximumLength(500).WithMessage("Translation short description cannot exceed 500 characters.");

            RuleFor(x => x.LongDescription)
                .NotEmpty().WithMessage("Translation long description is required.")
                .MaximumLength(5000).WithMessage("Translation long description cannot exceed 5000 characters.");

            RuleFor(x => x.MetaTitle)
                .MaximumLength(200).WithMessage("Translation meta title cannot exceed 200 characters.");

            RuleFor(x => x.MetaDescription)
                .MaximumLength(500).WithMessage("Translation meta description cannot exceed 500 characters.");
        }
    }
}
