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
    public class FAQRepository : RepositoryGeneric<FAQ>, IFAQRepository
    {
        public FAQRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<FAQ>> GetByTripIdAsync(
            Guid tripId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(f => f.TripId == tripId)
                .OrderBy(f => f.DisplayOrder)
                .ThenBy(f => f.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsByTripIdAsync(
            Guid tripId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(f => f.TripId == tripId, cancellationToken);
        }
    }
}