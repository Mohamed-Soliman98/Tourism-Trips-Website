using Application.DTOs.Banners;

namespace Application.Interfaces.Banners
{
    public interface IUpdateBannerService
    {
        Task<BannerUpdatedResponseDto?> UpdateBannerAsync(Guid id, UpdateBannerDto dto, CancellationToken cancellationToken);
    }
}