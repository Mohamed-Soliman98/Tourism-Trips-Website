using Application.DTOs.Trips;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Validators.Trips
{
    public class UpdateTripItineraryItemDtoValidator : AbstractValidator<UpdateTripItineraryItemDto>
    {
        public UpdateTripItineraryItemDtoValidator()
        {
            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Display order must be greater than or equal to 0.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Itinerary item title is required.")
                .MaximumLength(200).WithMessage("Itinerary item title cannot exceed 200 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(1000).WithMessage("Itinerary item description cannot exceed 1000 characters.");
        }
    }

    public class UpdateTripIncludeDtoValidator : AbstractValidator<UpdateTripIncludeDto>
    {
        public UpdateTripIncludeDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Include description is required.")
                .MaximumLength(500).WithMessage("Include description cannot exceed 500 characters.");
        }
    }

    public class UpdateTripExcludeDtoValidator : AbstractValidator<UpdateTripExcludeDto>
    {
        public UpdateTripExcludeDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Exclude description is required.")
                .MaximumLength(500).WithMessage("Exclude description cannot exceed 500 characters.");
        }
    }

    public class UpdateFAQTranslationDtoValidator : AbstractValidator<UpdateFAQTranslationDto>
    {
        public UpdateFAQTranslationDtoValidator()
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

    public class UpdateTripFAQDtoValidator : AbstractValidator<UpdateTripFAQDto>
    {
        public UpdateTripFAQDtoValidator()
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

            RuleForEach(x => x.Translations).SetValidator(new UpdateFAQTranslationDtoValidator());
        }
    }

    public class UpdateTripTranslationDtoValidator : AbstractValidator<UpdateTripTranslationDto>
    {
        public UpdateTripTranslationDtoValidator()
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

    public class UpdateTripDtoValidator : AbstractValidator<UpdateTripDto>
    {
        private static readonly Dictionary<string, string> AllowedFiles =
            new(StringComparer.OrdinalIgnoreCase)
            {
                [".png"] = "image/png",
                [".jpg"] = "image/jpeg",
                [".jpeg"] = "image/jpeg",
                [".webp"] = "image/webp"
            };

        private const long MaxFileSizeBytes = 5 * 1024 * 1024;

        public UpdateTripDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(200).WithMessage("Title cannot exceed 200 characters.");

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug is required.")
                .MaximumLength(200).WithMessage("Slug cannot exceed 200 characters.")
                .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
                .WithMessage("Slug must be lowercase alphanumeric characters separated by hyphens (e.g. 'cairo-day-tour').");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid trip status.");

            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage("Duration must be greater than 0.");

            RuleFor(x => x.DurationUnit)
                .IsInEnum().WithMessage("Invalid duration unit.");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Currency is required.")
                .MaximumLength(10).WithMessage("Currency cannot exceed 10 characters.");

            RuleFor(x => x.AdultPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Adult price cannot be negative.");

            RuleFor(x => x.ChildPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Child price cannot be negative.");

            RuleFor(x => x.OldPrice)
                .GreaterThanOrEqualTo(0).When(x => x.OldPrice.HasValue)
                .WithMessage("Old price cannot be negative.");

            RuleFor(x => x.ShortDescription)
                .NotEmpty().WithMessage("Short description is required.")
                .MaximumLength(500).WithMessage("Short description cannot exceed 500 characters.");

            RuleFor(x => x.LongDescription)
                .NotEmpty().WithMessage("Long description is required.")
                .MaximumLength(5000).WithMessage("Long description cannot exceed 5000 characters.");

            RuleFor(x => x.MetaTitle)
                .MaximumLength(200).WithMessage("Meta title cannot exceed 200 characters.");

            RuleFor(x => x.MetaDescription)
                .MaximumLength(500).WithMessage("Meta description cannot exceed 500 characters.");

            RuleFor(x => x.PickupLocation)
                .MaximumLength(500).WithMessage("Pickup location cannot exceed 500 characters.");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category ID is required.");

            RuleFor(x => x.DestinationId)
                .NotEmpty().WithMessage("Destination ID is required.");

            RuleFor(x => x.TourTypeId)
                .NotEmpty().WithMessage("Tour Type ID is required.");

            RuleFor(x => x.OgImage)
                .MustAsync(BeAValidImageFileAsync).When(x => x.OgImage != null)
                .WithMessage("OG image must be a valid non-empty image file (.jpg, .jpeg, .png, .webp) with matching MIME type and file signature up to 5MB.");

            RuleForEach(x => x.GalleryImages)
                .NotNull().WithMessage("Gallery image item cannot be null.")
                .MustAsync(BeAValidImageFileAsync).When(x => x.GalleryImages != null && x.GalleryImages.Count > 0)
                .WithMessage("Each gallery image must be a valid non-empty image file (.jpg, .jpeg, .png, .webp) with matching MIME type and file signature up to 5MB.");

            RuleFor(x => x.Translations)
                .Must(translations =>
                {
                    if (translations == null || translations.Count == 0) return true;
                    var languages = translations.Select(t => t.Language).ToList();
                    return languages.Count == languages.Distinct().Count();
                })
                .WithMessage("Duplicate translation languages are not allowed for the same Trip.");

            RuleForEach(x => x.ItineraryItems)
                .SetValidator(new UpdateTripItineraryItemDtoValidator())
                .When(x => x.ItineraryItems != null);

            RuleForEach(x => x.Includes)
                .SetValidator(new UpdateTripIncludeDtoValidator())
                .When(x => x.Includes != null);

            RuleForEach(x => x.Excludes)
                .SetValidator(new UpdateTripExcludeDtoValidator())
                .When(x => x.Excludes != null);

            RuleForEach(x => x.FAQs)
                .SetValidator(new UpdateTripFAQDtoValidator())
                .When(x => x.FAQs != null);

            RuleForEach(x => x.Translations)
                .SetValidator(new UpdateTripTranslationDtoValidator())
                .When(x => x.Translations != null);
        }

        private static async Task<bool> BeAValidImageFileAsync(IFormFile? file, CancellationToken cancellationToken)
        {
            if (file == null) return true;

            if (file.Length == 0 || file.Length > MaxFileSizeBytes)
                return false;

            var extension = Path.GetExtension(file.FileName);
            if (string.IsNullOrEmpty(extension) || !AllowedFiles.TryGetValue(extension, out var expectedMimeType))
                return false;

            if (!string.Equals(file.ContentType, expectedMimeType, StringComparison.OrdinalIgnoreCase))
                return false;

            return await ValidateFileSignatureAsync(file, extension, cancellationToken);
        }

        private static async Task<bool> ValidateFileSignatureAsync(IFormFile file, string extension, CancellationToken cancellationToken)
        {
            try
            {
                using var stream = file.OpenReadStream();
                var buffer = new byte[12];
                var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length, cancellationToken);
                if (bytesRead < 8) return false;

                var ext = extension.ToLowerInvariant();

                if (ext == ".png")
                {
                    return buffer[0] == 0x89 && buffer[1] == 0x50 && buffer[2] == 0x4E && buffer[3] == 0x47 &&
                           buffer[4] == 0x0D && buffer[5] == 0x0A && buffer[6] == 0x1A && buffer[7] == 0x0A;
                }

                if (ext == ".jpg" || ext == ".jpeg")
                {
                    return buffer[0] == 0xFF && buffer[1] == 0xD8 && buffer[2] == 0xFF;
                }

                if (ext == ".webp")
                {
                    if (bytesRead < 12) return false;
                    return buffer[0] == 0x52 && buffer[1] == 0x49 && buffer[2] == 0x46 && buffer[3] == 0x46 &&
                           buffer[8] == 0x57 && buffer[9] == 0x45 && buffer[10] == 0x42 && buffer[11] == 0x50;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
