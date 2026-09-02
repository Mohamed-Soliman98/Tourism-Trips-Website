using Application.DTOs.CMSSections;

namespace Application.Interfaces.CMSSections
{
    public interface IGetCMSSectionByIdService
    {
        Task<CMSSectionDetailDto?> GetCMSSectionByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}