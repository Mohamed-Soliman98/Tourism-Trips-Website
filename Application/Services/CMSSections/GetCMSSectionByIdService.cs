using Application.DTOs.CMSSections;
using Application.Interfaces.CMSSections;
using Application.Interfaces.IUnitOfWork;

namespace Application.Services.CMSSections
{
    public class GetCMSSectionByIdService : IGetCMSSectionByIdService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCMSSectionByIdService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CMSSectionDetailDto?> GetCMSSectionByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var cmsSection = await _unitOfWork.CMSSections.GetByIdAsync(id, cancellationToken);
            
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