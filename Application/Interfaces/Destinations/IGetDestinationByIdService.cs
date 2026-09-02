using Application.DTOs.Destinations;

namespace Application.Interfaces.Destinations
{
    public interface IGetDestinationByIdService
    {
        Task<DestinationDetailDto> GetDestinationByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}
