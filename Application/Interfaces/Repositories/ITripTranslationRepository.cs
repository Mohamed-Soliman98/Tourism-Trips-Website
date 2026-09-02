using Domain.Entitys;
using Domain.Enum;

namespace Application.Interfaces.Repositories
{
    public interface ITripTranslationRepository : IRepositoryGeneric<TripTranslation>
    {
        Task<List<TripTranslation>> GetByTripIdAsync(
            Guid tripId,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsByTripIdAndLanguageAsync(
            Guid tripId,
            Language language,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsByTripIdAndLanguageExcludingIdAsync(
            Guid tripId,
            Language language,
            Guid excludeId,
            CancellationToken cancellationToken = default);
    }
}
