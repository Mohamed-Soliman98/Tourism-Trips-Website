using Application.DTOs.SiteSettings;
using FluentValidation;

namespace Application.Validators.SiteSettings
{
    public class CreateSiteSettingDtoValidator : AbstractValidator<CreateSiteSettingDto>
    {
        public CreateSiteSettingDtoValidator()
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty().WithMessage("Company name is required.")
                .MaximumLength(200).WithMessage("Company name cannot exceed 200 characters.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone is required.")
                .MaximumLength(20).WithMessage("Phone cannot exceed 20 characters.");

            RuleFor(x => x.WhatsApp)
                .MaximumLength(20).WithMessage("WhatsApp cannot exceed 20 characters.")
                .When(x => !string.IsNullOrEmpty(x.WhatsApp));

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .MaximumLength(100).WithMessage("Email cannot exceed 100 characters.")
                .EmailAddress().WithMessage("Email must be a valid email address.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MaximumLength(500).WithMessage("Address cannot exceed 500 characters.");

            RuleFor(x => x.FacebookUrl)
                .MaximumLength(500).WithMessage("Facebook URL cannot exceed 500 characters.")
                .Must(BeValidUrlOrEmpty).WithMessage("Facebook URL must be a valid URL.")
                .When(x => !string.IsNullOrEmpty(x.FacebookUrl));

            RuleFor(x => x.InstagramUrl)
                .MaximumLength(500).WithMessage("Instagram URL cannot exceed 500 characters.")
                .Must(BeValidUrlOrEmpty).WithMessage("Instagram URL must be a valid URL.")
                .When(x => !string.IsNullOrEmpty(x.InstagramUrl));

            RuleFor(x => x.YouTubeUrl)
                .MaximumLength(500).WithMessage("YouTube URL cannot exceed 500 characters.")
                .Must(BeValidUrlOrEmpty).WithMessage("YouTube URL must be a valid URL.")
                .When(x => !string.IsNullOrEmpty(x.YouTubeUrl));

            RuleFor(x => x.TikTokUrl)
                .MaximumLength(500).WithMessage("TikTok URL cannot exceed 500 characters.")
                .Must(BeValidUrlOrEmpty).WithMessage("TikTok URL must be a valid URL.")
                .When(x => !string.IsNullOrEmpty(x.TikTokUrl));

            RuleFor(x => x.DefaultMetaTitle)
                .NotEmpty().WithMessage("Default meta title is required.")
                .MaximumLength(200).WithMessage("Default meta title cannot exceed 200 characters.");

            RuleFor(x => x.DefaultMetaDescription)
                .NotEmpty().WithMessage("Default meta description is required.")
                .MaximumLength(500).WithMessage("Default meta description cannot exceed 500 characters.");
        }

        private bool BeValidUrlOrEmpty(string? url)
        {
            return string.IsNullOrEmpty(url) || Uri.TryCreate(url, UriKind.Absolute, out _);
        }
    }
}