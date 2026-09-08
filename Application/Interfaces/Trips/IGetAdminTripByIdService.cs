using Application.DTOs.Trips;

namespace Application.Interfaces.Trips
{
    public interface IGetAdminTripByIdService
    {
        Task<AdminTripDetailsResponseDto> GetAdminTripByIdAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
