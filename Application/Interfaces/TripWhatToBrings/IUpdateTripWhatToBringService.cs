using Application.DTOs.TripWhatToBrings;
using Application.DTOs.Trips;

namespace Application.Interfaces.TripWhatToBrings
{
    public interface IUpdateTripWhatToBringService
    {
        Task<TripWhatToBringUpdatedResponseDto> UpdateTripWhatToBringAsync(
            Guid tripId,
            Guid whatToBringId,
            UpdateTripWhatToBringDto dto,
            CancellationToken cancellationToken = default);
    }
}