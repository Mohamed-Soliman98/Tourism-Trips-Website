using Application.Interfaces.Repositories;
using Domain.Entitys;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TripItineraryItemRepository : RepositoryGeneric<TripItineraryItem>, ITripItineraryItemRepository
    {
        public TripItineraryItemRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<TripItineraryItem>> GetByTripIdAsync(
            Guid tripId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(item => item.TripId == tripId)
                .OrderBy(item => item.DisplayOrder)
                .ThenBy(item => item.Id)
                .ToListAsync(cancellationToken);
        }
    }
}
