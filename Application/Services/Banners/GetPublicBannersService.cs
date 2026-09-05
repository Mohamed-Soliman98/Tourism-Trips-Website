using Application.DTOs.Banners;
using Application.Interfaces.Banners;
using Application.Interfaces.Repositories;

namespace Application.Services.Banners
{
    public class GetPublicBannersService : IGetPublicBannersService
    {
        private readonly IBannerRepository _bannerRepository;

        public GetPublicBannersService(IBannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }

        public async Task<List<PublicBannerDto>> GetPublicBannersAsync(CancellationToken cancellationToken = default)
        {
            var banners = await _bannerRepository.GetActiveBannersAsync(cancellationToken);

            return banners.Select(b => new PublicBannerDto(
                b.Id,
                b.Title,
                b.Description,
                b.ImageUrl,
                b.ButtonText,
                b.ButtonUrl,
                b.DisplayOrder
            )).ToList();
        }
    }
}
