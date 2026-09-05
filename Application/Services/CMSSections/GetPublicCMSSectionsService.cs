using Application.DTOs.CMSSections;
using Application.Interfaces.CMSSections;
using Application.Interfaces.Repositories;

namespace Application.Services.CMSSections
{
    public class GetPublicCMSSectionsService : IGetPublicCMSSectionsService
    {
        private readonly ICMSSectionRepository _cmsSectionRepository;

        public GetPublicCMSSectionsService(ICMSSectionRepository cmsSectionRepository)
        {
            _cmsSectionRepository = cmsSectionRepository;
        }

        public async Task<List<PublicCMSSectionDto>> GetPublicCMSSectionsAsync(CancellationToken cancellationToken = default)
        {
            var cmsSections = await _cmsSectionRepository.GetActiveCMSSectionsAsync(cancellationToken);

            return cmsSections.Select(c => new PublicCMSSectionDto(
                c.Id,
                c.Key,
                c.Title,
                c.Content,
                c.ImageUrl,
                c.DisplayOrder
            )).ToList();
        }
    }
}
