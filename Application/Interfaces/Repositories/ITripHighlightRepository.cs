using Domain.Entitys;

namespace Application.Interfaces.Repositories
{
    public interface ITripHighlightRepository : IRepositoryGeneric<TripHighlight>
    {
        Task<List<TripHighlight>> GetByTripIdAsync(
            Guid tripId,
            CancellationToken cancellationToken = default);

        Task<TripHighlight?> GetByIdWithTranslationsAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}