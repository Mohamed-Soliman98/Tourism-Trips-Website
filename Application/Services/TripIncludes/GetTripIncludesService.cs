using Application.DTOs.TripIncludes;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripIncludes;

namespace Application.Services.TripIncludes
{
    public class GetTripIncludesService : IGetTripIncludesService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripIncludeRepository _tripIncludeRepository;

        public GetTripIncludesService(
            ITripRepository tripRepository,
            ITripIncludeRepository tripIncludeRepository)
        {
            _tripRepository = tripRepository;
            _tripIncludeRepository = tripIncludeRepository;
        }

        public async Task<List<TripIncludeResponseDto>> GetTripIncludesAsync(
            Guid tripId,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var includes = await _tripIncludeRepository.GetByTripIdAsync(tripId, cancellationToken);

            return includes
                .Select(i => new TripIncludeResponseDto(i.Id, i.TripId, i.Description))
                .ToList();
        }
    }
}
