using Application.Interfaces.Repositories;
using Domain.Entitys;
using Domain.Enum;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TripTranslationRepository : RepositoryGeneric<TripTranslation>, ITripTranslationRepository
    {
        public TripTranslationRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<TripTranslation>> GetByTripIdAsync(
            Guid tripId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(t => t.TripId == tripId)
                .OrderBy(t => t.Language)
                .ThenBy(t => t.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<bool> ExistsByTripIdAndLanguageAsync(
            Guid tripId,
            Language language,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(
                t => t.TripId == tripId && t.Language == language,
                cancellationToken);
        }

        public async Task<bool> ExistsByTripIdAndLanguageExcludingIdAsync(
            Guid tripId,
            Language language,
            Guid excludeId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(
                t => t.TripId == tripId && t.Language == language && t.Id != excludeId,
                cancellationToken);
        }
    }
}
