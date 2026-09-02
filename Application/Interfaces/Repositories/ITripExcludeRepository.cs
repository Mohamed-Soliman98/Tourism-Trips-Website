using Domain.Entitys;

namespace Application.Interfaces.Repositories
{
    public interface ITripExcludeRepository : IRepositoryGeneric<TripExclude>
    {
        Task<List<TripExclude>> GetByTripIdAsync(
            Guid tripId,
            CancellationToken cancellationToken = default);
    }
}
