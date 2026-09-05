using Application.DTOs.CMSSections;
using Application.Interfaces.CMSSections;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;

namespace Application.Services.CMSSections
{
    public class UpdateCMSSectionService : IUpdateCMSSectionService
    {
        private readonly ICMSSectionRepository _cmsSectionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCMSSectionService(
            IUnitOfWork unitOfWork,
            ICMSSectionRepository cmsSectionRepository)
        {
            _unitOfWork = unitOfWork;
            _cmsSectionRepository = cmsSectionRepository;
        }

        public async Task<CMSSectionUpdatedResponseDto?> UpdateCMSSectionAsync(Guid id, UpdateCMSSectionDto dto, CancellationToken cancellationToken)
        {
            var cmsSection = await _cmsSectionRepository.GetByIdAsync(id, cancellationToken);
            
            if (cmsSection == null)
                return null;

            if (cmsSection.Key != dto.Key)
            {
                var keyExists = await _cmsSectionRepository.KeyExistsAsync(dto.Key, id, cancellationToken);
                if (keyExists)
                {
                    throw new InvalidOperationException($"A CMS section with key '{dto.Key}' already exists.");
                }
            }

            cmsSection.Key = dto.Key;
            cmsSection.Title = dto.Title;
            cmsSection.Content = dto.Content;
            cmsSection.ImageUrl = dto.ImageUrl;
            cmsSection.DisplayOrder = dto.DisplayOrder;
            cmsSection.IsActive = dto.IsActive;
            cmsSection.UpdatedAt = DateTime.UtcNow;

            _cmsSectionRepository.Update(cmsSection);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CMSSectionUpdatedResponseDto(
                cmsSection.Id,
                cmsSection.Key,
                cmsSection.Title,
                cmsSection.Content,
                cmsSection.ImageUrl,
                cmsSection.DisplayOrder,
                cmsSection.IsActive,
                cmsSection.UpdatedAt.Value
            );
        }
    }
}