using Application.DTOs.TripWhatToBrings;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripWhatToBrings;
using Domain.Enum;

namespace Application.Services.TripWhatToBrings
{
    public class GetTripWhatToBringByIdService : IGetTripWhatToBringByIdService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripWhatToBringRepository _tripWhatToBringRepository;

        public GetTripWhatToBringByIdService(
            ITripRepository tripRepository,
            ITripWhatToBringRepository tripWhatToBringRepository)
        {
            _tripRepository = tripRepository;
            _tripWhatToBringRepository = tripWhatToBringRepository;
        }

        public async Task<TripWhatToBringResponseDto> GetTripWhatToBringByIdAsync(
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

            var item = await _tripWhatToBringRepository.GetByIdWithTranslationsAsync(whatToBringId, cancellationToken);
            if (item == null)
                throw new KeyNotFoundException($"Trip what-to-bring item with ID '{whatToBringId}' was not found.");

            if (item.TripId != tripId)
                throw new InvalidOperationException(
                    $"Trip what-to-bring item '{whatToBringId}' does not belong to trip '{tripId}'.");

            // Get description from the English translation, fall back to German
            var englishTranslation = item.Translations?.FirstOrDefault(t => t.Language == Language.English);
            var description = englishTranslation?.Description ?? item.Translations?.FirstOrDefault(t => t.Language == Language.German)?.Description ?? string.Empty;

            return new TripWhatToBringResponseDto(item.Id, item.TripId, description);
        }
    }
}