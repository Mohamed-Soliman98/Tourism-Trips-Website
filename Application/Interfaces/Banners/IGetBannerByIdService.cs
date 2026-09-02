using Application.DTOs.Banners;

namespace Application.Interfaces.Banners
{
    public interface IGetBannerByIdService
    {
        Task<BannerDetailDto?> GetBannerByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}