using Application.DTOs.CMSSections;
using Application.Interfaces.CMSSections;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Domain.Entity;

namespace Application.Services.CMSSections
{
    public class CreateCMSSectionService : ICreateCMSSectionService
    {
        private readonly ICMSSectionRepository _cmsSectionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateCMSSectionService(
            IUnitOfWork unitOfWork,
            ICMSSectionRepository cmsSectionRepository)
        {
            _unitOfWork = unitOfWork;
            _cmsSectionRepository = cmsSectionRepository;
        }

        public async Task<CMSSectionCreatedResponseDto> CreateCMSSectionAsync(CreateCMSSectionDto dto, CancellationToken cancellationToken)
        {
            var keyExists = await _cmsSectionRepository.KeyExistsAsync(dto.Key, cancellationToken: cancellationToken);
            if (keyExists)
            {
                throw new InvalidOperationException($"A CMS section with key '{dto.Key}' already exists.");
            }

            var cmsSection = new CMSSection
            {
                Id = Guid.NewGuid(),
                Key = dto.Key,
                Title = dto.Title,
                Content = dto.Content,
                ImageUrl = dto.ImageUrl,
                DisplayOrder = dto.DisplayOrder,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            _cmsSectionRepository.Add(cmsSection);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CMSSectionCreatedResponseDto(
                cmsSection.Id,
                cmsSection.Key,
                cmsSection.Title,
                cmsSection.Content,
                cmsSection.ImageUrl,
                cmsSection.DisplayOrder,
                cmsSection.IsActive,
                cmsSection.CreatedAt
            );
        }
    }
}