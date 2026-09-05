using Application.DTOs.Banners;
using Application.DTOs.Common;
using Application.Interfaces.Banners;
using Application.Interfaces.Repositories;

namespace Application.Services.Banners
{
    public class GetBannersService : IGetBannersService
    {
        private readonly IBannerRepository _bannerRepository;

        public GetBannersService(IBannerRepository bannerRepository)
        {
            _bannerRepository = bannerRepository;
        }

        public async Task<PagedResult<BannerSummaryDto>> GetBannersAsync(GetBannersQueryDto query, CancellationToken cancellationToken)
        {
            return await _bannerRepository.GetBannersAsync(query, cancellationToken);
        }
    }
}