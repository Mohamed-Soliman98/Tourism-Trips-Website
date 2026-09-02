using Application.DTOs.Banners;
using FluentValidation;

namespace Application.Validators.Banners
{
    public class UpdateBannerDtoValidator : AbstractValidator<UpdateBannerDto>
    {
        public UpdateBannerDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters.");

            RuleFor(x => x.ImageUrl)
                .NotEmpty().WithMessage("Image URL is required.")
                .MaximumLength(500).WithMessage("Image URL cannot exceed 500 characters.")
                .Must(BeValidUrl).WithMessage("Image URL must be a valid URL.");

            RuleFor(x => x.ButtonText)
                .MaximumLength(100).WithMessage("Button text cannot exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.ButtonText));

            RuleFor(x => x.ButtonUrl)
                .MaximumLength(500).WithMessage("Button URL cannot exceed 500 characters.")
                .Must(BeValidUrlOrEmpty).WithMessage("Button URL must be a valid URL.")
                .When(x => !string.IsNullOrEmpty(x.ButtonUrl));

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Display order must be greater than or equal to 0.");
        }

        private bool BeValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out _) || url.StartsWith("/");
        }

        private bool BeValidUrlOrEmpty(string? url)
        {
            return string.IsNullOrEmpty(url) || Uri.TryCreate(url, UriKind.Absolute, out _) || url.StartsWith("/");
        }
    }
}