using Application.DTOs.Banners;

namespace Application.Interfaces.Banners
{
    public interface IGetPublicBannersService
    {
        Task<List<PublicBannerDto>> GetPublicBannersAsync(CancellationToken cancellationToken = default);
    }
}
