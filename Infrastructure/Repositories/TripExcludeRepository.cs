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
                .Include(e => e.Translations)
                .Where(e => e.TripId == tripId)
                .OrderBy(e => e.CreatedAt)
                .ThenBy(e => e.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<TripExclude?> GetByIdWithTranslationsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(e => e.Translations)
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }
    }
}
