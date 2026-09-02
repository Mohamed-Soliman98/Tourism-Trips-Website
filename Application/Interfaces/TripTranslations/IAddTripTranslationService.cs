using Application.DTOs.TripTranslations;
using Application.DTOs.Trips;

namespace Application.Interfaces.TripTranslations
{
    public interface IAddTripTranslationService
    {
        Task<TripTranslationAddedResponseDto> AddTripTranslationAsync(
            Guid tripId,
            CreateTripTranslationDto dto,
            CancellationToken cancellationToken = default);
    }
}
