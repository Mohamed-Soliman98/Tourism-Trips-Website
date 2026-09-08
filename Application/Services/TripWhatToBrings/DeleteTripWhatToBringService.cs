using Application.DTOs.TripWhatToBrings;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripWhatToBrings;

namespace Application.Services.TripWhatToBrings
{
    public class DeleteTripWhatToBringService : IDeleteTripWhatToBringService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripWhatToBringRepository _tripWhatToBringRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTripWhatToBringService(
            IUnitOfWork unitOfWork,
            ITripRepository tripRepository,
            ITripWhatToBringRepository tripWhatToBringRepository)
        {
            _unitOfWork = unitOfWork;
            _tripRepository = tripRepository;
            _tripWhatToBringRepository = tripWhatToBringRepository;
        }

        public async Task<TripWhatToBringDeletedResponseDto> DeleteTripWhatToBringAsync(
            Guid tripId,
            Guid whatToBringId,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            if (whatToBringId == Guid.Empty)
                throw new ArgumentException("What-to-bring Id cannot be empty.", nameof(whatToBringId));

            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var item = await _tripWhatToBringRepository.GetByIdAsync(whatToBringId, cancellationToken);
            if (item == null)
                throw new KeyNotFoundException($"Trip what-to-bring item with ID '{whatToBringId}' was not found.");

            if (item.TripId != tripId)
                throw new InvalidOperationException(
                    $"Trip what-to-bring item '{whatToBringId}' does not belong to trip '{tripId}'.");

            _tripWhatToBringRepository.Remove(item);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TripWhatToBringDeletedResponseDto();
        }
    }
}