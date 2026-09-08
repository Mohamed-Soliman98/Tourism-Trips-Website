using Application.DTOs.TripWhatToBrings;

namespace Application.Interfaces.TripWhatToBrings
{
    public interface IDeleteTripWhatToBringService
    {
        Task<TripWhatToBringDeletedResponseDto> DeleteTripWhatToBringAsync(
            Guid tripId,
            Guid whatToBringId,
            CancellationToken cancellationToken = default);
    }
}