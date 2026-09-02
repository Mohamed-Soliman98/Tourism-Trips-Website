using Application.DTOs.Trips;

namespace Application.Interfaces.Trips
{
    public interface IDeleteTripService
    {
        Task<TripDeletedResponseDto> DeleteTripAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
