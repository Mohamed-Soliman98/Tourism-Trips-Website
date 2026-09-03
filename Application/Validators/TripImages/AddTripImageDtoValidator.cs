using Application.DTOs.TripImages;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Application.Validators.TripImages
{
    public class AddTripImageDtoValidator : AbstractValidator<AddTripImageDto>
    {
        private static readonly Dictionary<string, string> AllowedFiles =
            new(StringComparer.OrdinalIgnoreCase)
            {
                [".png"]  = "image/png",
                [".jpg"]  = "image/jpeg",
                [".jpeg"] = "image/jpeg",
                [".webp"] = "image/webp"
            };

        private const long MaxFileSizeBytes = 5 * 1024 * 1024;

        public AddTripImageDtoValidator()
        {
            RuleFor(x => x.ImageFile)
                .NotNull().WithMessage("An image file is required.")
                .MustAsync(BeAValidImageFileAsync).When(x => x.ImageFile != null)
                .WithMessage("Image must be a valid non-empty file (.jpg, .jpeg, .png, .webp) " +
                             "with matching MIME type and file signature, up to 5 MB.");

            RuleFor(x => x.AltText)
                .MaximumLength(200).WithMessage("Alt text cannot exceed 200 characters.")
                .When(x => x.AltText != null);

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Display order must be 0 or greater.");
        }

        private static async Task<bool> BeAValidImageFileAsync(
            IFormFile? file,
            CancellationToken cancellationToken)
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

        private static async Task<bool> ValidateFileSignatureAsync(
            IFormFile file,
            string extension,
            CancellationToken cancellationToken)
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
