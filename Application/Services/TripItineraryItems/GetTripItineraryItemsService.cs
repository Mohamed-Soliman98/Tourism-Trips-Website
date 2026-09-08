using Application.DTOs.TripItineraryItems;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripItineraryItems;
using Domain.Enum;

namespace Application.Services.TripItineraryItems
{
    public class GetTripItineraryItemsService : IGetTripItineraryItemsService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripItineraryItemRepository _tripItineraryItemRepository;

        public GetTripItineraryItemsService(
            ITripRepository tripRepository,
            ITripItineraryItemRepository tripItineraryItemRepository)
        {
            _tripRepository = tripRepository;
            _tripItineraryItemRepository = tripItineraryItemRepository;
        }

        public async Task<List<TripItineraryItemResponseDto>> GetTripItineraryItemsAsync(
            Guid tripId,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var items = await _tripItineraryItemRepository.GetByTripIdAsync(tripId, cancellationToken);

            return items
                .Select(i =>
                {
                    var translation = i.Translations?.FirstOrDefault(t => t.Language == Language.English);
                    return new TripItineraryItemResponseDto(
                        i.Id,
                        i.TripId,
                        i.DisplayOrder,
                        translation?.Title ?? string.Empty,
                        translation?.Description);
                })
                .ToList();
        }
    }
}