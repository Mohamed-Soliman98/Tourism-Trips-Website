using Application.DTOs.CMSSections;
using Application.Interfaces.CMSSections;
using Application.Interfaces.Repositories;

namespace Application.Services.CMSSections
{
    public class GetCMSSectionByIdService : IGetCMSSectionByIdService
    {
        private readonly ICMSSectionRepository _cmsSectionRepository;

        public GetCMSSectionByIdService(ICMSSectionRepository cmsSectionRepository)
        {
            _cmsSectionRepository = cmsSectionRepository;
        }

        public async Task<CMSSectionDetailDto?> GetCMSSectionByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var cmsSection = await _cmsSectionRepository.GetByIdAsync(id, cancellationToken);
            
            if (cmsSection == null)
                return null;

            return new CMSSectionDetailDto(
                cmsSection.Id,
                cmsSection.Key,
                cmsSection.Title,
                cmsSection.Content,
                cmsSection.ImageUrl,
                cmsSection.DisplayOrder,
                cmsSection.IsActive,
                cmsSection.CreatedAt,
                cmsSection.UpdatedAt
            );
        }
    }
}