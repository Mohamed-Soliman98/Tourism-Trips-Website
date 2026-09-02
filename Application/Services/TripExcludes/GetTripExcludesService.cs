using Application.DTOs.TripExcludes;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.TripExcludes;

namespace Application.Services.TripExcludes
{
    public class GetTripExcludesService : IGetTripExcludesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTripExcludesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TripExcludeResponseDto>> GetTripExcludesAsync(
            Guid tripId,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            var trip = await _unitOfWork.Trips.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var excludes = await _unitOfWork.TripExcludes.GetByTripIdAsync(tripId, cancellationToken);

            return excludes
                .Select(e => new TripExcludeResponseDto(e.Id, e.TripId, e.Description))
                .ToList();
        }
    }
}
