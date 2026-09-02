using Application.DTOs.CMSSections;

namespace Application.Interfaces.CMSSections
{
    public interface IDeleteCMSSectionService
    {
        Task<CMSSectionDeletedResponseDto?> DeleteCMSSectionAsync(Guid id, CancellationToken cancellationToken);
    }
}