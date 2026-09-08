using Application.DTOs.TripWhatToBrings;

namespace Application.Interfaces.TripWhatToBrings
{
    public interface IGetTripWhatToBringsService
    {
        Task<List<TripWhatToBringResponseDto>> GetTripWhatToBringsAsync(
            Guid tripId,
            CancellationToken cancellationToken = default);
    }
}