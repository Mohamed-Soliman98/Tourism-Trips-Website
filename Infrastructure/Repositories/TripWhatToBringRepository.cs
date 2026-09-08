using Application.Interfaces.Repositories;
using Domain.Entitys;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TripWhatToBringRepository : RepositoryGeneric<TripWhatToBring>, ITripWhatToBringRepository
    {
        public TripWhatToBringRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<TripWhatToBring>> GetByTripIdAsync(
            Guid tripId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(w => w.Translations)
                .Where(w => w.TripId == tripId)
                .OrderBy(w => w.DisplayOrder)
                .ThenBy(w => w.Id)
                .ToListAsync(cancellationToken);
        }

        public async Task<TripWhatToBring?> GetByIdWithTranslationsAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .Include(w => w.Translations)
                .FirstOrDefaultAsync(w => w.Id == id, cancellationToken);
        }
    }
}