using Application.DTOs.Banners;

namespace Application.Interfaces.Banners
{
    public interface ICreateBannerService
    {
        Task<BannerCreatedResponseDto> CreateBannerAsync(CreateBannerDto dto, CancellationToken cancellationToken);
    }
}