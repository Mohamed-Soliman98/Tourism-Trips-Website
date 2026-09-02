using Application.DTOs.CMSSections;

namespace Application.Interfaces.CMSSections
{
    public interface IGetPublicCMSSectionsService
    {
        Task<List<PublicCMSSectionDto>> GetPublicCMSSectionsAsync(CancellationToken cancellationToken = default);
    }
}
