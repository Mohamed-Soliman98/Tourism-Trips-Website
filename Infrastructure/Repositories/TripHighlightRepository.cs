using Application.Interfaces.Repositories;
using Domain.Entitys;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TripHighlightRepository : RepositoryGeneric<TripHighlight>, ITripHighlightRepository
    {
        public TripHighlightRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<TripHighlight>> GetByTripIdAsync(
            Guid tripId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(h => h.Translations)
                .Where(h => h.TripId == tripId)
                .OrderBy(h => h.DisplayOrder)
                .ThenBy(h => h.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<TripHighlight?> GetByIdWithTranslationsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(h => h.Translations)
                .FirstOrDefaultAsync(h => h.Id == id, cancellationToken);
        }
    }
}