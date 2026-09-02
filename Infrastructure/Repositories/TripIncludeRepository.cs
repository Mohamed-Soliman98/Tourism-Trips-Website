using Application.Interfaces.Repositories;
using Domain.Entitys;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TripIncludeRepository : RepositoryGeneric<TripInclude>, ITripIncludeRepository
    {
        public TripIncludeRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<TripInclude>> GetByTripIdAsync(
            Guid tripId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(i => i.TripId == tripId)
                .OrderBy(i => i.CreatedAt)
                .ThenBy(i => i.Id)
                .ToListAsync(cancellationToken);
        }
    }
}
