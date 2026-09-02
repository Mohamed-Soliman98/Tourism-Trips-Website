using Application.DTOs.Destinations;

namespace Application.Interfaces.Destinations
{
    public interface IUpdateDestinationService
    {
        Task<DestinationUpdatedResponseDto> UpdateDestinationAsync(
            Guid id,
            UpdateDestinationDto dto,
            CancellationToken cancellationToken = default);
    }
}
