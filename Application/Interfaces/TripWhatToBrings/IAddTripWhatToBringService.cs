using Application.DTOs.TripWhatToBrings;
using Application.DTOs.Trips;

namespace Application.Interfaces.TripWhatToBrings
{
    public interface IAddTripWhatToBringService
    {
        Task<TripWhatToBringAddedResponseDto> AddTripWhatToBringAsync(
            Guid tripId,
            CreateTripWhatToBringDto dto,
            CancellationToken cancellationToken = default);
    }
}