using Application.DTOs.CMSSections;
using Application.DTOs.Common;
using Application.Interfaces.CMSSections;
using Application.Interfaces.Repositories;

namespace Application.Services.CMSSections
{
    public class GetCMSSectionsService : IGetCMSSectionsService
    {
        private readonly ICMSSectionRepository _cmsSectionRepository;

        public GetCMSSectionsService(ICMSSectionRepository cmsSectionRepository)
        {
            _cmsSectionRepository = cmsSectionRepository;
        }

        public async Task<PagedResult<CMSSectionSummaryDto>> GetCMSSectionsAsync(GetCMSSectionsQueryDto query, CancellationToken cancellationToken)
        {
            return await _cmsSectionRepository.GetCMSSectionsAsync(query, cancellationToken);
        }
    }
}