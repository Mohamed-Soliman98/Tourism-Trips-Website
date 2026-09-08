using Domain.Entitys;

namespace Application.Interfaces.Repositories
{
    public interface ITripWhatToBringRepository : IRepositoryGeneric<TripWhatToBring>
    {
        Task<List<TripWhatToBring>> GetByTripIdAsync(
            Guid tripId,
            CancellationToken cancellationToken = default);

        Task<TripWhatToBring?> GetByIdWithTranslationsAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}