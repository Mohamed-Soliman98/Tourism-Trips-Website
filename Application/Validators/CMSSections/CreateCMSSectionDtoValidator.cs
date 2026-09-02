using Application.DTOs.CMSSections;
using FluentValidation;

namespace Application.Validators.CMSSections
{
    public class CreateCMSSectionDtoValidator : AbstractValidator<CreateCMSSectionDto>
    {
        public CreateCMSSectionDtoValidator()
        {
            RuleFor(x => x.Key)
                .NotEmpty().WithMessage("Key is required.")
                .MaximumLength(100).WithMessage("Key cannot exceed 100 characters.")
                .Matches("^[a-zA-Z0-9_-]+$").WithMessage("Key can only contain letters, numbers, underscores, and hyphens.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MaximumLength(5000).WithMessage("Content cannot exceed 5000 characters.");

            RuleFor(x => x.ImageUrl)
                .MaximumLength(500).WithMessage("Image URL cannot exceed 500 characters.")
                .Must(BeValidUrlOrEmpty).WithMessage("Image URL must be a valid URL.")
                .When(x => !string.IsNullOrEmpty(x.ImageUrl));

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Display order must be greater than or equal to 0.");
        }

        private bool BeValidUrlOrEmpty(string? url)
        {
            return string.IsNullOrEmpty(url) || Uri.TryCreate(url, UriKind.Absolute, out _) || url.StartsWith("/");
        }
    }
}