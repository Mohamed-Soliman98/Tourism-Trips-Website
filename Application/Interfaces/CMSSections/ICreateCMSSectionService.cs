using Application.DTOs.CMSSections;

namespace Application.Interfaces.CMSSections
{
    public interface ICreateCMSSectionService
    {
        Task<CMSSectionCreatedResponseDto> CreateCMSSectionAsync(CreateCMSSectionDto dto, CancellationToken cancellationToken);
    }
}