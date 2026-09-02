using Application.DTOs.CMSSections;
using Application.DTOs.Common;
using Domain.Entity;

namespace Application.Interfaces.Repositories
{
    public interface ICMSSectionRepository : IRepositoryGeneric<CMSSection>
    {
        Task<PagedResult<CMSSectionSummaryDto>> GetCMSSectionsAsync(GetCMSSectionsQueryDto query, CancellationToken cancellationToken);
        Task<bool> KeyExistsAsync(string key, Guid? excludeId = null, CancellationToken cancellationToken = default);
        Task<List<CMSSection>> GetActiveCMSSectionsAsync(CancellationToken cancellationToken = default);
    }
}