using Application.DTOs.TripWhatToBrings;

namespace Application.Interfaces.TripWhatToBrings
{
    public interface IGetTripWhatToBringByIdService
    {
        Task<TripWhatToBringResponseDto> GetTripWhatToBringByIdAsync(
            Guid tripId,
            Guid whatToBringId,
            CancellationToken cancellationToken = default);
    }
}