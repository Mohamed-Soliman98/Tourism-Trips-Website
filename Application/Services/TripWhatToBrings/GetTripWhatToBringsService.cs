using Application.DTOs.TripWhatToBrings;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripWhatToBrings;
using Domain.Enum;

namespace Application.Services.TripWhatToBrings
{
    public class GetTripWhatToBringsService : IGetTripWhatToBringsService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripWhatToBringRepository _tripWhatToBringRepository;

        public GetTripWhatToBringsService(
            ITripRepository tripRepository,
            ITripWhatToBringRepository tripWhatToBringRepository)
        {
            _tripRepository = tripRepository;
            _tripWhatToBringRepository = tripWhatToBringRepository;
        }

        public async Task<List<TripWhatToBringResponseDto>> GetTripWhatToBringsAsync(
            Guid tripId,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var items = await _tripWhatToBringRepository.GetByTripIdAsync(tripId, cancellationToken);

            return items
                .Select(w => new TripWhatToBringResponseDto(
                    w.Id,
                    w.TripId,
                    w.Translations?.FirstOrDefault(t => t.Language == Language.English)?.Description ?? string.Empty))
                .ToList();
        }
    }
}