using Application.DTOs.Banners;
using Application.DTOs.Common;
using Application.Interfaces.Repositories;
using Domain.Entity;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BannerRepository : RepositoryGeneric<Banner>, IBannerRepository
    {
        public BannerRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PagedResult<BannerSummaryDto>> GetBannersAsync(GetBannersQueryDto query, CancellationToken cancellationToken)
        {
            var queryable = _dbSet.AsQueryable();

            if (query.IsActive.HasValue)
            {
                queryable = queryable.Where(b => b.IsActive == query.IsActive.Value);
            }

            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                var searchTerm = query.SearchTerm.ToLower();
                queryable = queryable.Where(b => 
                    b.Title.ToLower().Contains(searchTerm) ||
                    b.Description.ToLower().Contains(searchTerm) ||
                    (b.ButtonText != null && b.ButtonText.ToLower().Contains(searchTerm)));
            }

            var totalCount = await queryable.CountAsync(cancellationToken);

            var banners = await queryable
                .OrderBy(b => b.DisplayOrder)
                .ThenByDescending(b => b.CreatedAt)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(b => new BannerSummaryDto(
                    b.Id,
                    b.Title,
                    b.Description,
                    b.ImageUrl,
                    b.ButtonText,
                    b.ButtonUrl,
                    b.DisplayOrder,
                    b.IsActive,
                    b.CreatedAt
                ))
                .ToListAsync(cancellationToken);

            return new PagedResult<BannerSummaryDto>(
                banners,
                totalCount,
                query.Page,
                query.PageSize
            );
        }

        public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(cancellationToken);
        }

        public async Task<int> GetActiveCountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(b => b.IsActive, cancellationToken);
        }

        public async Task<int> GetInactiveCountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(b => !b.IsActive, cancellationToken);
        }

        public async Task<List<Banner>> GetActiveBannersAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(b => b.IsActive)
                .OrderBy(b => b.DisplayOrder)
                .ThenByDescending(b => b.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}