using Application.DTOs.TripHighlights;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripHighlights;
using Domain.Enum;

namespace Application.Services.TripHighlights
{
    public class GetTripHighlightsService : IGetTripHighlightsService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripHighlightRepository _tripHighlightRepository;

        public GetTripHighlightsService(
            ITripRepository tripRepository,
            ITripHighlightRepository tripHighlightRepository)
        {
            _tripRepository = tripRepository;
            _tripHighlightRepository = tripHighlightRepository;
        }

        public async Task<List<TripHighlightResponseDto>> GetTripHighlightsAsync(
            Guid tripId,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var highlights = await _tripHighlightRepository.GetByTripIdAsync(tripId, cancellationToken);

            return highlights
                .Select(h => new TripHighlightResponseDto(
                    h.Id,
                    h.TripId,
                    h.Translations?.FirstOrDefault(t => t.Language == Language.English)?.Description ?? string.Empty))
                .ToList();
        }
    }
}