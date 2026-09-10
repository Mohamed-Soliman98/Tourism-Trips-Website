using Application.DTOs.Trips;
using Application.Interfaces.Repositories;
using Domain.Entity;
using Domain.Enum;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TripRepository : RepositoryGeneric<Trip>, ITripRepository
    {
        public TripRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistsBySlugAsync(string slug,CancellationToken cancellationToken)
        {
            var normalizedSlug = slug.Trim().ToLowerInvariant();
            return await _dbSet.AnyAsync(x => x.Slug.ToLower() == normalizedSlug, cancellationToken);
        }

        public async Task<Trip?> GetByIdWithDetailsAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .AsSplitQuery()
                .Include(t => t.Category)
                .Include(t => t.Destination)
                .Include(t => t.TourType)
                .Include(t => t.Images)
                .Include(t => t.ItineraryItems)
                    .ThenInclude(i => i.Translations)
                .Include(t => t.Includes)
                    .ThenInclude(i => i.Translations)
                .Include(t => t.Excludes)
                    .ThenInclude(e => e.Translations)
                .Include(t => t.Highlights)
                    .ThenInclude(h => h.Translations)
                .Include(t => t.WhatToBringItems)
                    .ThenInclude(w => w.Translations)
                .Include(t => t.FAQs)
                    .ThenInclude(f => f.Translations)
                .Include(t => t.Translations)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<Trip?> GetBySlugWithDetailsAsync(string slug, CancellationToken cancellationToken = default)
        {
            var normalizedSlug = slug.Trim().ToLowerInvariant();

            return await _dbSet
                .AsNoTracking()
                .AsSplitQuery()
                .Include(t => t.Category)
                .Include(t => t.Destination)
                .Include(t => t.TourType)
                .Include(t => t.Images)
                .Include(t => t.ItineraryItems)
                    .ThenInclude(i => i.Translations)
                .Include(t => t.Includes)
                    .ThenInclude(i => i.Translations)
                .Include(t => t.Excludes)
                    .ThenInclude(e => e.Translations)
                .Include(t => t.Highlights)
                    .ThenInclude(h => h.Translations)
                .Include(t => t.WhatToBringItems)
                    .ThenInclude(w => w.Translations)
                .Include(t => t.FAQs)
                    .ThenInclude(f => f.Translations)
                .Include(t => t.Translations)
                .FirstOrDefaultAsync(t => t.Slug.ToLower() == normalizedSlug && t.Status == TripStatus.Active, cancellationToken);
        }

        public async Task<(List<Trip> Items, int TotalCount)> GetPublicTripsAsync(GetPublicTripsQueryDto query,CancellationToken cancellationToken = default)
        {
            var queryable = _dbSet
                .AsNoTracking()
                .Where(t => t.Status == TripStatus.Active);

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var searchTerm = query.Search.Trim();
                queryable = queryable.Where(t =>
                    EF.Functions.Like(t.Title, $"%{searchTerm}%") ||
                    EF.Functions.Like(t.ShortDescription, $"%{searchTerm}%"));
            }

            if (query.CategoryId.HasValue && query.CategoryId.Value != Guid.Empty)
            {
                queryable = queryable.Where(t => t.CategoryId == query.CategoryId.Value);
            }

            if (query.DestinationId.HasValue && query.DestinationId.Value != Guid.Empty)
            {
                queryable = queryable.Where(t => t.DestinationId == query.DestinationId.Value);
            }

            if (query.IsFeatured.HasValue)
            {
                queryable = queryable.Where(t => t.IsFeatured == query.IsFeatured.Value);
            }

            var totalCount = await queryable.CountAsync(cancellationToken);

            var items = await queryable
                .Include(t => t.Category)
                .Include(t => t.Destination)
                .Include(t => t.TourType)
                .Include(t => t.Images.Where(i => i.IsCover))
                .Include(t => t.Translations)
                .OrderBy(t => t.DisplayOrder)
                .ThenBy(t => t.Id)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<(List<Trip> Items, int TotalCount)> GetAdminTripsAsync( GetAdminTripsQueryDto query,CancellationToken cancellationToken = default)
        {
            var queryable = _dbSet
                .AsNoTracking();

            if (query.Status.HasValue)
            {
                queryable = queryable.Where(t => t.Status == query.Status.Value);
            }

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var searchTerm = query.Search.Trim();
                queryable = queryable.Where(t =>
                    EF.Functions.Like(t.Title, $"%{searchTerm}%") ||
                    EF.Functions.Like(t.Slug, $"%{searchTerm}%"));
            }

            if (query.CategoryId.HasValue && query.CategoryId.Value != Guid.Empty)
            {
                queryable = queryable.Where(t => t.CategoryId == query.CategoryId.Value);
            }

            if (query.DestinationId.HasValue && query.DestinationId.Value != Guid.Empty)
            {
                queryable = queryable.Where(t => t.DestinationId == query.DestinationId.Value);
            }

            if (query.TourTypeId.HasValue && query.TourTypeId.Value != Guid.Empty)
            {
                queryable = queryable.Where(t => t.TourTypeId == query.TourTypeId.Value);
            }

            if (query.IsFeatured.HasValue)
            {
                queryable = queryable.Where(t => t.IsFeatured == query.IsFeatured.Value);
            }

            if (query.MinPrice.HasValue)
            {
                queryable = queryable.Where(t => t.AdultPrice >= query.MinPrice.Value);
            }

            if (query.MaxPrice.HasValue)
            {
                queryable = queryable.Where(t => t.AdultPrice <= query.MaxPrice.Value);
            }

            var totalCount = await queryable.CountAsync(cancellationToken);

            var sortBy = (query.SortBy ?? "DisplayOrder").Trim().ToLowerInvariant();
            var isDescending = !string.IsNullOrEmpty(query.SortDirection) &&
                (query.SortDirection.Trim().Equals("desc", StringComparison.OrdinalIgnoreCase) ||
                 query.SortDirection.Trim().Equals("descending", StringComparison.OrdinalIgnoreCase));

            queryable = (sortBy, isDescending) switch
            {
                ("createdat", true) => queryable.OrderByDescending(t => t.CreatedAt).ThenByDescending(t => t.Id),
                ("createdat", false) => queryable.OrderBy(t => t.CreatedAt).ThenBy(t => t.Id),
                ("adultprice", true) => queryable.OrderByDescending(t => t.AdultPrice).ThenByDescending(t => t.Id),
                ("adultprice", false) => queryable.OrderBy(t => t.AdultPrice).ThenBy(t => t.Id),
                ("displayorder", true) => queryable.OrderByDescending(t => t.DisplayOrder).ThenByDescending(t => t.Id),
                _ => queryable.OrderBy(t => t.DisplayOrder).ThenBy(t => t.Id)
            };

            var items = await queryable
                .Include(t => t.Category)
                .Include(t => t.Destination)
                .Include(t => t.TourType)
                .Include(t => t.Images.Where(i => i.IsCover))
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<Trip?> GetByIdForUpdateAsync(Guid id,CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsSplitQuery()
                .Include(t => t.Category)
                .Include(t => t.Destination)
                .Include(t => t.TourType)
                .Include(t => t.Images)
                .Include(t => t.ItineraryItems)
                    .ThenInclude(i => i.Translations)
                .Include(t => t.Includes)
                    .ThenInclude(i => i.Translations)
                .Include(t => t.Excludes)
                    .ThenInclude(e => e.Translations)
                .Include(t => t.Highlights)
                    .ThenInclude(h => h.Translations)
                .Include(t => t.WhatToBringItems)
                    .ThenInclude(w => w.Translations)
                .Include(t => t.FAQs)
                    .ThenInclude(f => f.Translations)
                .Include(t => t.Translations)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<bool> ExistsBySlugOtherThanIdAsync( string slug, Guid id, CancellationToken cancellationToken = default)
        {
            var normalizedSlug = slug.Trim().ToLowerInvariant();
            return await _dbSet.AnyAsync(x => x.Slug.ToLower() == normalizedSlug && x.Id != id, cancellationToken);
        }

        public async Task<Trip?> GetByIdForDeleteAsync(Guid id, CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsSplitQuery()
                .Include(t => t.Images)
                .Include(t => t.ItineraryItems)
                    .ThenInclude(i => i.Translations)
                .Include(t => t.Includes)
                    .ThenInclude(i => i.Translations)
                .Include(t => t.Excludes)
                    .ThenInclude(e => e.Translations)
                .Include(t => t.Highlights)
                    .ThenInclude(h => h.Translations)
                .Include(t => t.WhatToBringItems)
                    .ThenInclude(w => w.Translations)
                .Include(t => t.FAQs)
                    .ThenInclude(f => f.Translations)
                .Include(t => t.Translations)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(cancellationToken);
        }

        public async Task<int> GetCountByStatusAsync(TripStatus status, CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(t => t.Status == status, cancellationToken);
        }
    }
}
