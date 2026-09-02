using Application.DTOs.TripIncludes;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.TripIncludes;

namespace Application.Services.TripIncludes
{
    public class GetTripIncludeByIdService : IGetTripIncludeByIdService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTripIncludeByIdService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TripIncludeResponseDto> GetTripIncludeByIdAsync(
            Guid tripId,
            Guid includeId,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            if (includeId == Guid.Empty)
                throw new ArgumentException("Include Id cannot be empty.", nameof(includeId));

            var trip = await _unitOfWork.Trips.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var include = await _unitOfWork.TripIncludes.GetByIdAsync(includeId, cancellationToken);
            if (include == null)
                throw new KeyNotFoundException($"Trip include with ID '{includeId}' was not found.");

            if (include.TripId != tripId)
                throw new InvalidOperationException(
                    $"Trip include '{includeId}' does not belong to trip '{tripId}'.");

            return new TripIncludeResponseDto(include.Id, include.TripId, include.Description);
        }
    }
}
