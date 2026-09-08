using Domain.Entitys;

namespace Application.Interfaces.Repositories
{
    public interface ITripIncludeRepository : IRepositoryGeneric<TripInclude>
    {
        Task<List<TripInclude>> GetByTripIdAsync(
            Guid tripId,
            CancellationToken cancellationToken = default);

        Task<TripInclude?> GetByIdWithTranslationsAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}
