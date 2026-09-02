using Application.DTOs.CMSSections;

namespace Application.Interfaces.CMSSections
{
    public interface IUpdateCMSSectionService
    {
        Task<CMSSectionUpdatedResponseDto?> UpdateCMSSectionAsync(Guid id, UpdateCMSSectionDto dto, CancellationToken cancellationToken);
    }
}