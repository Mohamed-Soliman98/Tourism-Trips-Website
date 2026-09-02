using Application.DTOs.Media;
using Application.Interfaces.Media;
using Application.Interfaces.Storage;
using FluentValidation;

namespace Application.Services.Media
{
    public class UploadMediaService : IUploadMediaService
    {
        private readonly IFileStorageService _fileStorage;
        private readonly IValidator<UploadMediaDto> _validator;

        // Folder name used inside wwwroot/uploads/
        private const string MediaFolder = "media";

        public UploadMediaService(
            IFileStorageService fileStorage,
            IValidator<UploadMediaDto> validator)
        {
            _fileStorage = fileStorage;
            _validator = validator;
        }

        public async Task<MediaUploadedResponseDto> UploadAsync(
            UploadMediaDto dto,
            CancellationToken cancellationToken = default)
        {
            // 1. Validate (extension, size, MIME, magic bytes)
            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            // 2. Persist file via existing IFileStorageService → returns relative path
            //    e.g. "uploads/media/3f2a1c9b-….jpg"
            var relativePath = await _fileStorage.SaveAsync(dto.File, MediaFolder, cancellationToken);

            // 3. Extract the server-generated filename from the relative path
            var fileName = Path.GetFileName(relativePath);

            return new MediaUploadedResponseDto(
                Url: relativePath,
                FileName: fileName
            );
        }
    }
}
