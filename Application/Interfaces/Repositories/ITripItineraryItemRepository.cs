using Domain.Entitys;

namespace Application.Interfaces.Repositories
{
    public interface ITripItineraryItemRepository : IRepositoryGeneric<TripItineraryItem>
    {
        Task<List<TripItineraryItem>> GetByTripIdAsync(
            Guid tripId,
            CancellationToken cancellationToken = default);
    }
}
