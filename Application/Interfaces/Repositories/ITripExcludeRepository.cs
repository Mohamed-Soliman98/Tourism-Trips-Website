using Domain.Entitys;

namespace Application.Interfaces.Repositories
{
    public interface ITripExcludeRepository : IRepositoryGeneric<TripExclude>
    {
        Task<List<TripExclude>> GetByTripIdAsync(
            Guid tripId,
            CancellationToken cancellationToken = default);

        Task<TripExclude?> GetByIdWithTranslationsAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}
