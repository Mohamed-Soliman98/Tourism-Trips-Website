using Application.DTOs.TripIncludes;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripIncludes;

namespace Application.Services.TripIncludes
{
    public class GetTripIncludeByIdService : IGetTripIncludeByIdService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripIncludeRepository _tripIncludeRepository;

        public GetTripIncludeByIdService(
            ITripRepository tripRepository,
            ITripIncludeRepository tripIncludeRepository)
        {
            _tripRepository = tripRepository;
            _tripIncludeRepository = tripIncludeRepository;
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

            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var include = await _tripIncludeRepository.GetByIdAsync(includeId, cancellationToken);
            if (include == null)
                throw new KeyNotFoundException($"Trip include with ID '{includeId}' was not found.");

            if (include.TripId != tripId)
                throw new InvalidOperationException(
                    $"Trip include '{includeId}' does not belong to trip '{tripId}'.");

            return new TripIncludeResponseDto(include.Id, include.TripId, include.Description);
        }
    }
}
