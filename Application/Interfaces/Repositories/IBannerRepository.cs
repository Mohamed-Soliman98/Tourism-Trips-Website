using Application.DTOs.Banners;
using Application.DTOs.Common;
using Domain.Entity;

namespace Application.Interfaces.Repositories
{
    public interface IBannerRepository : IRepositoryGeneric<Banner>
    {
        Task<PagedResult<BannerSummaryDto>> GetBannersAsync(GetBannersQueryDto query, CancellationToken cancellationToken);

        Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);

        Task<int> GetActiveCountAsync(CancellationToken cancellationToken = default);

        Task<int> GetInactiveCountAsync(CancellationToken cancellationToken = default);

        Task<List<Banner>> GetActiveBannersAsync(CancellationToken cancellationToken = default);
    }
}