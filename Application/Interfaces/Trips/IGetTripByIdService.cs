using Application.DTOs.Trips;

namespace Application.Interfaces.Trips
{
    public interface IGetTripByIdService
    {
        Task<TripDetailsResponseDto> GetTripByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
