using Application.DTOs.TripTranslations;

namespace Application.Interfaces.TripTranslations
{
    public interface IGetTripTranslationsService
    {
        Task<List<TripTranslationResponseDto>> GetTripTranslationsAsync(
            Guid tripId,
            CancellationToken cancellationToken = default);
    }
}
