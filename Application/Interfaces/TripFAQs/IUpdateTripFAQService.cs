using Application.DTOs.TripFAQs;
using Application.DTOs.Trips;

namespace Application.Interfaces.TripFAQs
{
    public interface IUpdateTripFAQService
    {
        Task<TripFAQUpdatedResponseDto> UpdateTripFAQAsync(
            Guid tripId,
            Guid faqId,
            UpdateTripFAQDto dto,
            CancellationToken cancellationToken = default);
    }
}