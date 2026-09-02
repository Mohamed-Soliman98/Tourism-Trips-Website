using Application.DTOs.CMSSections;
using Application.Interfaces.CMSSections;
using Application.Interfaces.IUnitOfWork;

namespace Application.Services.CMSSections
{
    public class GetPublicCMSSectionsService : IGetPublicCMSSectionsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPublicCMSSectionsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PublicCMSSectionDto>> GetPublicCMSSectionsAsync(CancellationToken cancellationToken = default)
        {
            var cmsSections = await _unitOfWork.CMSSections.GetActiveCMSSectionsAsync(cancellationToken);

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
