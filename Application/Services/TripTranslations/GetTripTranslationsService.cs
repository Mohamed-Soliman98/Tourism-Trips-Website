using Application.DTOs.TripTranslations;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripTranslations;

namespace Application.Services.TripTranslations
{
    public class GetTripTranslationsService : IGetTripTranslationsService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripTranslationRepository _tripTranslationRepository;

        public GetTripTranslationsService(
            ITripRepository tripRepository,
            ITripTranslationRepository tripTranslationRepository)
        {
            _tripRepository = tripRepository;
            _tripTranslationRepository = tripTranslationRepository;
        }

        public async Task<List<TripTranslationResponseDto>> GetTripTranslationsAsync(
            Guid tripId,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var translations = await _tripTranslationRepository.GetByTripIdAsync(tripId, cancellationToken);

            return translations
                .Select(t => new TripTranslationResponseDto(
                    t.Id,
                    t.TripId,
                    t.Language,
                    t.Title,
                    t.ShortDescription,
                    t.LongDescription,
                    t.MetaTitle,
                    t.MetaDescription))
                .ToList();
        }
    }
}
