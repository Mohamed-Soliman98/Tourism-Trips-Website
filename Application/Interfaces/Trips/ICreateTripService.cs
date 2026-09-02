using Application.DTOs.Trips;

namespace Application.Interfaces.Trips
{
    public interface ICreateTripService
    {
        Task<TripCreatedResponseDto> CreateTripAsync(CreateTripDto dto, CancellationToken cancellationToken = default);
    }
}
