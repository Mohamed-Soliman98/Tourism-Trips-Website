using Application.Interfaces.Repositories;
using Domain.Entitys;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class TripImageRepository : RepositoryGeneric<TripImage>, ITripImageRepository
    {
        public TripImageRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<TripImage>> GetByTripIdAsync(
            Guid tripId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(i => i.TripId == tripId)
                .OrderBy(i => i.DisplayOrder)
                .ThenBy(i => i.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsByTripIdAsync(
            Guid tripId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(i => i.TripId == tripId, cancellationToken);
        }
        
        public async Task<List<TripImage>> GetCoverImagesTrackedAsync(
            Guid tripId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Where(i => i.TripId == tripId && i.IsCover)
                .ToListAsync(cancellationToken);
        }
    }
}
