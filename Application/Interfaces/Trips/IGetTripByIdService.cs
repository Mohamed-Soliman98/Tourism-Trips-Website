using Application.DTOs.Trips;
using Domain.Enum;

namespace Application.Interfaces.Trips
{
    public interface IGetTripByIdService
    {
        Task<PublicTripDetailsResponseDto> GetTripByIdAsync(Guid id, Language? language, CancellationToken cancellationToken = default);
    }
}
