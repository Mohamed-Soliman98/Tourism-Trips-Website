using Application.DTOs.TripTranslations;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripTranslations;

namespace Application.Services.TripTranslations
{
    public class GetTripTranslationByIdService : IGetTripTranslationByIdService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripTranslationRepository _tripTranslationRepository;

        public GetTripTranslationByIdService(
            ITripRepository tripRepository,
            ITripTranslationRepository tripTranslationRepository)
        {
            _tripRepository = tripRepository;
            _tripTranslationRepository = tripTranslationRepository;
        }

        public async Task<TripTranslationResponseDto> GetTripTranslationByIdAsync(
            Guid tripId,
            Guid translationId,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            if (translationId == Guid.Empty)
                throw new ArgumentException("Translation Id cannot be empty.", nameof(translationId));

            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var translation = await _tripTranslationRepository.GetByIdAsync(translationId, cancellationToken);
            if (translation == null)
                throw new KeyNotFoundException($"Trip translation with ID '{translationId}' was not found.");

            if (translation.TripId != tripId)
                throw new InvalidOperationException(
                    $"Trip translation '{translationId}' does not belong to trip '{tripId}'.");

            return new TripTranslationResponseDto(
                translation.Id,
                translation.TripId,
                translation.Language,
                translation.Title,
                translation.ShortDescription,
                translation.LongDescription,
                translation.MetaTitle,
                translation.MetaDescription);
        }
    }
}
