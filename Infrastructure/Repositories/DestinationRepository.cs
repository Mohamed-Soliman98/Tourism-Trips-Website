using Application.DTOs.Destinations;
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
    public class DestinationRepository : RepositoryGeneric<Destination>, IDestinationRepository
    {
        public DestinationRepository(AppDbContext context) : base(context)
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

        public async Task<(List<Destination> Items, int TotalCount)> GetDestinationsAsync(
            GetDestinationsQueryDto query,
            CancellationToken cancellationToken = default)
        {
            var q = _dbSet.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.Trim().ToLower();
                q = q.Where(d => d.Name.ToLower().Contains(search));
            }

            if (query.IsActive.HasValue)
            {
                q = q.Where(d => d.IsActive == query.IsActive.Value);
            }

            var totalCount = await q.CountAsync(cancellationToken);

            var items = await q
                .OrderBy(d => d.Name)
                .ThenBy(d => d.Id)
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
                d => d.Name.ToLower() == normalizedName && d.Id != excludeId,
                cancellationToken);
        }

        public async Task<List<Destination>> GetActiveAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(d => d.IsActive)
                .OrderBy(d => d.Name)
                .ThenBy(d => d.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> HasTripsAsync(
            Guid destinationId,
            CancellationToken cancellationToken = default)
        {
            return await _context.Set<Trip>()
                .AnyAsync(t => t.DestinationId == destinationId, cancellationToken);
        }
    }
}

