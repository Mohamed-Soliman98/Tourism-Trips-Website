using Application.DTOs.TripTranslations;
using Application.DTOs.Trips;

namespace Application.Interfaces.TripTranslations
{
    public interface IUpdateTripTranslationService
    {
        Task<TripTranslationUpdatedResponseDto> UpdateTripTranslationAsync(
            Guid tripId,
            Guid translationId,
            UpdateTripTranslationDto dto,
            CancellationToken cancellationToken = default);
    }
}
