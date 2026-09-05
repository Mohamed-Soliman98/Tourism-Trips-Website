using Application.DTOs.TripExcludes;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripExcludes;

namespace Application.Services.TripExcludes
{
    public class GetTripExcludesService : IGetTripExcludesService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripExcludeRepository _tripExcludeRepository;

        public GetTripExcludesService(
            ITripRepository tripRepository,
            ITripExcludeRepository tripExcludeRepository)
        {
            _tripRepository = tripRepository;
            _tripExcludeRepository = tripExcludeRepository;
        }

        public async Task<List<TripExcludeResponseDto>> GetTripExcludesAsync(
            Guid tripId,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var excludes = await _tripExcludeRepository.GetByTripIdAsync(tripId, cancellationToken);

            return excludes
                .Select(e => new TripExcludeResponseDto(e.Id, e.TripId, e.Description))
                .ToList();
        }
    }
}
