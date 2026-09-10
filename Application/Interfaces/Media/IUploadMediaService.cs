using Application.DTOs.Media;

namespace Application.Interfaces.Media
{
    public interface IUploadMediaService
    {
        Task<MediaUploadedResponseDto> UploadAsync( UploadMediaDto dto,CancellationToken cancellationToken = default);
    }
}
