using Application.DTOs.SiteSettings;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.SiteSettings;
using Domain.Entity;

namespace Application.Services.SiteSettings
{
    public class CreateSiteSettingsService : ICreateSiteSettingsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateSiteSettingsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SiteSettingCreatedResponseDto> CreateSiteSettingsAsync(CreateSiteSettingDto dto, CancellationToken cancellationToken)
        {
            // Check if settings already exist (singleton pattern)
            var settingsExist = await _unitOfWork.SiteSettings.AnySettingsExistAsync(cancellationToken);
            if (settingsExist)
            {
                throw new InvalidOperationException("Site settings already exist. Use the update endpoint to modify existing settings.");
            }

            var siteSetting = new SiteSetting
            {
                Id = Guid.NewGuid(),
                CompanyName = dto.CompanyName,
                Phone = dto.Phone,
                WhatsApp = dto.WhatsApp,
                Email = dto.Email,
                Address = dto.Address,
                FacebookUrl = dto.FacebookUrl,
                InstagramUrl = dto.InstagramUrl,
                YouTubeUrl = dto.YouTubeUrl,
                TikTokUrl = dto.TikTokUrl,
                DefaultMetaTitle = dto.DefaultMetaTitle,
                DefaultMetaDescription = dto.DefaultMetaDescription,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            _unitOfWork.SiteSettings.Add(siteSetting);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new SiteSettingCreatedResponseDto(
                siteSetting.Id,
                siteSetting.CompanyName,
                siteSetting.Phone,
                siteSetting.WhatsApp,
                siteSetting.Email,
                siteSetting.Address,
                siteSetting.FacebookUrl,
                siteSetting.InstagramUrl,
                siteSetting.YouTubeUrl,
                siteSetting.TikTokUrl,
                siteSetting.DefaultMetaTitle,
                siteSetting.DefaultMetaDescription,
                siteSetting.CreatedAt
            );
        }
    }
}