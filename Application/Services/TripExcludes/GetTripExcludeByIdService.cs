using Application.DTOs.TripExcludes;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.TripExcludes;

namespace Application.Services.TripExcludes
{
    public class GetTripExcludeByIdService : IGetTripExcludeByIdService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTripExcludeByIdService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TripExcludeResponseDto> GetTripExcludeByIdAsync(
            Guid tripId,
            Guid excludeId,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            if (excludeId == Guid.Empty)
                throw new ArgumentException("Exclude Id cannot be empty.", nameof(excludeId));

            var trip = await _unitOfWork.Trips.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var exclude = await _unitOfWork.TripExcludes.GetByIdAsync(excludeId, cancellationToken);
            if (exclude == null)
                throw new KeyNotFoundException($"Trip exclude with ID '{excludeId}' was not found.");

            if (exclude.TripId != tripId)
                throw new InvalidOperationException(
                    $"Trip exclude '{excludeId}' does not belong to trip '{tripId}'.");

            return new TripExcludeResponseDto(exclude.Id, exclude.TripId, exclude.Description);
        }
    }
}
