using Application.DTOs.TripHighlights;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripHighlights;
using Domain.Enum;

namespace Application.Services.TripHighlights
{
    public class GetTripHighlightByIdService : IGetTripHighlightByIdService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripHighlightRepository _tripHighlightRepository;

        public GetTripHighlightByIdService(
            ITripRepository tripRepository,
            ITripHighlightRepository tripHighlightRepository)
        {
            _tripRepository = tripRepository;
            _tripHighlightRepository = tripHighlightRepository;
        }

        public async Task<TripHighlightResponseDto> GetTripHighlightByIdAsync(
            Guid tripId,
            Guid highlightId,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            if (highlightId == Guid.Empty)
                throw new ArgumentException("Highlight Id cannot be empty.", nameof(highlightId));

            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var highlight = await _tripHighlightRepository.GetByIdWithTranslationsAsync(highlightId, cancellationToken);
            if (highlight == null)
                throw new KeyNotFoundException($"Trip highlight with ID '{highlightId}' was not found.");

            if (highlight.TripId != tripId)
                throw new InvalidOperationException(
                    $"Trip highlight '{highlightId}' does not belong to trip '{tripId}'.");

            // Get description from the English translation, fall back to German
            var englishTranslation = highlight.Translations?.FirstOrDefault(t => t.Language == Language.English);
            var description = englishTranslation?.Description ?? highlight.Translations?.FirstOrDefault(t => t.Language == Language.German)?.Description ?? string.Empty;

            return new TripHighlightResponseDto(highlight.Id, highlight.TripId, description);
        }
    }
}