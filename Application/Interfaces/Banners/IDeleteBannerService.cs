using Application.DTOs.Banners;

namespace Application.Interfaces.Banners
{
    public interface IDeleteBannerService
    {
        Task<BannerDeletedResponseDto?> DeleteBannerAsync(Guid id, CancellationToken cancellationToken);
    }
}