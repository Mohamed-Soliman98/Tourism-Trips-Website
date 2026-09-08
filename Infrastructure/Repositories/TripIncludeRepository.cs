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
                .Include(i => i.Translations)
                .Where(i => i.TripId == tripId)
                .OrderBy(i => i.CreatedAt)
                .ThenBy(i => i.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<TripInclude?> GetByIdWithTranslationsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(i => i.Translations)
                .FirstOrDefaultAsync(i => i.Id == id, cancellationToken);
        }
    }
}
