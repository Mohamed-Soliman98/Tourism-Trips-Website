using Application.DTOs.Banners;
using Application.DTOs.Common;

namespace Application.Interfaces.Banners
{
    public interface IGetBannersService
    {
        Task<PagedResult<BannerSummaryDto>> GetBannersAsync(GetBannersQueryDto query, CancellationToken cancellationToken);
    }
}