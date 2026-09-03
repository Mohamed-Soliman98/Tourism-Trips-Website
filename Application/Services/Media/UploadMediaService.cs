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
            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            var relativePath = await _fileStorage.SaveAsync(dto.File, MediaFolder, cancellationToken);

            var fileName = Path.GetFileName(relativePath);

            return new MediaUploadedResponseDto(
                Url: relativePath,
                FileName: fileName
            );
        }
    }
}
