using Application.DTOs.TourTypes;
using Application.Interfaces.Repositories;
using Domain.Entity;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TourTypeRepository : RepositoryGeneric<TourType>, ITourTypeRepository
    {
        public TourTypeRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistsAndIsActiveAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            return await _dbSet.AnyAsync(x => x.Id == id && x.IsActive, cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            var normalizedName = name.Trim().ToLower();
            return await _dbSet.AnyAsync(x => x.Name.ToLower() == normalizedName, cancellationToken);
        }

        public async Task<(List<TourType> Items, int TotalCount)> GetTourTypesAsync(
            GetTourTypesQueryDto query,
            CancellationToken cancellationToken = default)
        {
            var q = _dbSet.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.Trim().ToLower();
                q = q.Where(t => t.Name.ToLower().Contains(search));
            }

            if (query.IsActive.HasValue)
            {
                q = q.Where(t => t.IsActive == query.IsActive.Value);
            }

            var totalCount = await q.CountAsync(cancellationToken);

            var items = await q
                .OrderBy(t => t.Name)
                .ThenBy(t => t.Id)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<bool> ExistsByNameExcludingIdAsync(
            string name,
            Guid excludeId,
            CancellationToken cancellationToken = default)
        {
            var normalizedName = name.Trim().ToLower();
            return await _dbSet.AnyAsync(
                t => t.Name.ToLower() == normalizedName && t.Id != excludeId,
                cancellationToken);
        }

        public async Task<List<TourType>> GetActiveAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(t => t.IsActive)
                .OrderBy(t => t.Name)
                .ThenBy(t => t.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> HasTripsAsync(
            Guid tourTypeId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Trip>()
                .AnyAsync(t => t.TourTypeId == tourTypeId, cancellationToken);
        }
    }
}

