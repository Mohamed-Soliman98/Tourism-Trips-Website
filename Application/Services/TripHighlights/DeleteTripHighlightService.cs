using Application.DTOs.TripHighlights;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripHighlights;

namespace Application.Services.TripHighlights
{
    public class DeleteTripHighlightService : IDeleteTripHighlightService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripHighlightRepository _tripHighlightRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTripHighlightService(
            IUnitOfWork unitOfWork,
            ITripRepository tripRepository,
            ITripHighlightRepository tripHighlightRepository)
        {
            _unitOfWork = unitOfWork;
            _tripRepository = tripRepository;
            _tripHighlightRepository = tripHighlightRepository;
        }

        public async Task<TripHighlightDeletedResponseDto> DeleteTripHighlightAsync(
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

            var highlight = await _tripHighlightRepository.GetByIdAsync(highlightId, cancellationToken);
            if (highlight == null)
                throw new KeyNotFoundException($"Trip highlight with ID '{highlightId}' was not found.");

            if (highlight.TripId != tripId)
                throw new InvalidOperationException(
                    $"Trip highlight '{highlightId}' does not belong to trip '{tripId}'.");

            _tripHighlightRepository.Remove(highlight);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TripHighlightDeletedResponseDto();
        }
    }
}