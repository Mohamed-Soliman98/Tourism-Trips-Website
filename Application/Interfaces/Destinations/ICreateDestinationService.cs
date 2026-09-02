using Application.DTOs.Destinations;

namespace Application.Interfaces.Destinations
{
    public interface ICreateDestinationService
    {
        Task<DestinationCreatedResponseDto> CreateDestinationAsync(CreateDestinationDto dto, CancellationToken cancellationToken = default);
    }
}