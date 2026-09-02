using Application.DTOs.TripTranslations;

namespace Application.Interfaces.TripTranslations
{
    public interface IGetTripTranslationByIdService
    {
        Task<TripTranslationResponseDto> GetTripTranslationByIdAsync(
            Guid tripId,
            Guid translationId,
            CancellationToken cancellationToken = default);
    }
}
