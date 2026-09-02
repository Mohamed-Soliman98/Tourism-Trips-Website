using Application.DTOs.TripIncludes;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.TripIncludes;

namespace Application.Services.TripIncludes
{
    public class GetTripIncludesService : IGetTripIncludesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTripIncludesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<TripIncludeResponseDto>> GetTripIncludesAsync(
            Guid tripId,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            var trip = await _unitOfWork.Trips.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var includes = await _unitOfWork.TripIncludes.GetByTripIdAsync(tripId, cancellationToken);

            return includes
                .Select(i => new TripIncludeResponseDto(i.Id, i.TripId, i.Description))
                .ToList();
        }
    }
}
