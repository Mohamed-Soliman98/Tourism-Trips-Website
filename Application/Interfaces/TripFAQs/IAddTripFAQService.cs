using Application.DTOs.TripFAQs;
using Application.DTOs.Trips;

namespace Application.Interfaces.TripFAQs
{
    public interface IAddTripFAQService
    {
        Task<TripFAQAddedResponseDto> AddTripFAQAsync(
            Guid tripId,
            CreateTripFAQDto dto,
            CancellationToken cancellationToken = default);
    }
}