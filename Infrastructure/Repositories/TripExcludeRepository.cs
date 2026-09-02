using Application.Interfaces.Repositories;
using Domain.Entitys;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TripExcludeRepository : RepositoryGeneric<TripExclude>, ITripExcludeRepository
    {
        public TripExcludeRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<TripExclude>> GetByTripIdAsync(
            Guid tripId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(e => e.TripId == tripId)
                .OrderBy(e => e.CreatedAt)
                .ThenBy(e => e.Id)
                .ToListAsync(cancellationToken);
        }
    }
}
