using Application.DTOs.Banners;
using Application.Interfaces.Banners;
using Application.Interfaces.Repositories;

namespace Application.Services.Banners
{
    public class GetBannerByIdService : IGetBannerByIdService
    {
        private readonly IBannerRepository _bannerRepository;

        public GetBannerByIdService(IBannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }

        public async Task<BannerDetailDto?> GetBannerByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var banner = await _bannerRepository.GetByIdAsync(id, cancellationToken);
            
            if (banner == null)
                return null;

            return new BannerDetailDto(
                banner.Id,
                banner.Title,
                banner.Description,
                banner.ImageUrl,
                banner.ButtonText,
                banner.ButtonUrl,
                banner.DisplayOrder,
                banner.IsActive,
                banner.CreatedAt,
                banner.UpdatedAt
            );
        }
    }
}