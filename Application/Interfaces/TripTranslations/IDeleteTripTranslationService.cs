using Application.DTOs.TripTranslations;

namespace Application.Interfaces.TripTranslations
{
    public interface IDeleteTripTranslationService
    {
        Task<TripTranslationDeletedResponseDto> DeleteTripTranslationAsync(
            Guid tripId,
            Guid translationId,
            CancellationToken cancellationToken = default);
    }
}
