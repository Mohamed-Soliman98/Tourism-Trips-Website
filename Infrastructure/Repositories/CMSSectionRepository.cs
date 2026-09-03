using Application.DTOs.CMSSections;
using Application.DTOs.Common;
using Application.Interfaces.Repositories;
using Domain.Entity;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class CMSSectionRepository : RepositoryGeneric<CMSSection>, ICMSSectionRepository
    {
        public CMSSectionRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PagedResult<CMSSectionSummaryDto>> GetCMSSectionsAsync(GetCMSSectionsQueryDto query, CancellationToken cancellationToken)
        {
            var queryable = _dbSet.AsQueryable();

            if (query.IsActive.HasValue)
            {
                queryable = queryable.Where(c => c.IsActive == query.IsActive.Value);
            }

            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                var searchTerm = query.SearchTerm.ToLower();
                queryable = queryable.Where(c => 
                    c.Key.ToLower().Contains(searchTerm) ||
                    c.Title.ToLower().Contains(searchTerm) ||
                    c.Content.ToLower().Contains(searchTerm));
            }

            var totalCount = await queryable.CountAsync(cancellationToken);

            var cmsSections = await queryable
                .OrderBy(c => c.DisplayOrder)
                .ThenByDescending(c => c.CreatedAt)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(c => new CMSSectionSummaryDto(
                    c.Id,
                    c.Key,
                    c.Title,
                    c.Content,
                    c.ImageUrl,
                    c.DisplayOrder,
                    c.IsActive,
                    c.CreatedAt
                ))
                .ToListAsync(cancellationToken);

            return new PagedResult<CMSSectionSummaryDto>(
                cmsSections,
                totalCount,
                query.Page,
                query.PageSize
            );
        }

        public async Task<bool> KeyExistsAsync(string key, Guid? excludeId = null, CancellationToken cancellationToken = default)
        {
            var query = _dbSet.Where(c => c.Key == key);
            
            if (excludeId.HasValue)
            {
                query = query.Where(c => c.Id != excludeId.Value);
            }

            return await query.AnyAsync(cancellationToken);
        }

        public async Task<List<CMSSection>> GetActiveCMSSectionsAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(c => c.IsActive)
                .OrderBy(c => c.DisplayOrder)
                .ThenByDescending(c => c.CreatedAt)
                .ToListAsync(cancellationToken);
        }
    }
}