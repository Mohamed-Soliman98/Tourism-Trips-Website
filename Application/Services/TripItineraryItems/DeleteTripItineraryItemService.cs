using Application.DTOs.TripItineraryItems;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripItineraryItems;

namespace Application.Services.TripItineraryItems
{
    public class DeleteTripItineraryItemService : IDeleteTripItineraryItemService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripItineraryItemRepository _tripItineraryItemRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTripItineraryItemService(
            IUnitOfWork unitOfWork,
            ITripRepository tripRepository,
            ITripItineraryItemRepository tripItineraryItemRepository)
        {
            _unitOfWork = unitOfWork;
            _tripRepository = tripRepository;
            _tripItineraryItemRepository = tripItineraryItemRepository;
        }

        public async Task<TripItineraryItemDeletedResponseDto> DeleteTripItineraryItemAsync(
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

            var item = await _tripItineraryItemRepository.GetByIdAsync(itineraryItemId, cancellationToken);
            if (item == null)
                throw new KeyNotFoundException($"Trip itinerary item with ID '{itineraryItemId}' was not found.");

            if (item.TripId != tripId)
                throw new InvalidOperationException(
                    $"Trip itinerary item '{itineraryItemId}' does not belong to trip '{tripId}'.");

            _tripItineraryItemRepository.Remove(item);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TripItineraryItemDeletedResponseDto();
        }
    }
}