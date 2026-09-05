using Application.DTOs.TripExcludes;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripExcludes;

namespace Application.Services.TripExcludes
{
    public class GetTripExcludeByIdService : IGetTripExcludeByIdService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripExcludeRepository _tripExcludeRepository;

        public GetTripExcludeByIdService(
            ITripRepository tripRepository,
            ITripExcludeRepository tripExcludeRepository)
        {
            _tripRepository = tripRepository;
            _tripExcludeRepository = tripExcludeRepository;
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

            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var exclude = await _tripExcludeRepository.GetByIdAsync(excludeId, cancellationToken);
            if (exclude == null)
                throw new KeyNotFoundException($"Trip exclude with ID '{excludeId}' was not found.");

            if (exclude.TripId != tripId)
                throw new InvalidOperationException(
                    $"Trip exclude '{excludeId}' does not belong to trip '{tripId}'.");

            return new TripExcludeResponseDto(exclude.Id, exclude.TripId, exclude.Description);
        }
    }
}
