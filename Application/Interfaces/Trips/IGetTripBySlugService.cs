using Application.DTOs.Trips;
using Domain.Enum;

namespace Application.Interfaces.Trips
{
    public interface IGetTripBySlugService
    {
        Task<TripDetailsResponseDto> GetTripBySlugAsync(string slug, Language language, CancellationToken cancellationToken = default);
    }
}
