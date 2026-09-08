using Application.DTOs.TripItineraryItems;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripItineraryItems;
using Domain.Enum;

namespace Application.Services.TripItineraryItems
{
    public class GetTripItineraryItemByIdService : IGetTripItineraryItemByIdService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripItineraryItemRepository _tripItineraryItemRepository;

        public GetTripItineraryItemByIdService(
            ITripRepository tripRepository,
            ITripItineraryItemRepository tripItineraryItemRepository)
        {
            _tripRepository = tripRepository;
            _tripItineraryItemRepository = tripItineraryItemRepository;
        }

        public async Task<TripItineraryItemResponseDto> GetTripItineraryItemByIdAsync(
            Guid tripId,
            Guid itineraryItemId,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            if (itineraryItemId == Guid.Empty)
                throw new ArgumentException("Itinerary item Id cannot be empty.", nameof(itineraryItemId));

            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var item = await _tripItineraryItemRepository.GetByIdWithTranslationsAsync(itineraryItemId, cancellationToken);
            if (item == null)
                throw new KeyNotFoundException($"Trip itinerary item with ID '{itineraryItemId}' was not found.");

            if (item.TripId != tripId)
                throw new InvalidOperationException(
                    $"Trip itinerary item '{itineraryItemId}' does not belong to trip '{tripId}'.");

            // Get title from the English translation, fall back to German
            var englishTranslation = item.Translations?.FirstOrDefault(t => t.Language == Language.English)
                ?? item.Translations?.FirstOrDefault(t => t.Language == Language.German);

            return new TripItineraryItemResponseDto(
                item.Id,
                item.TripId,
                item.DisplayOrder,
                englishTranslation?.Title ?? string.Empty,
                englishTranslation?.Description);
        }
    }
}