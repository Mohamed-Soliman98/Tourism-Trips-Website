using Application.DTOs.SiteSettings;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.SiteSettings;
using Domain.Entity;

namespace Application.Services.SiteSettings
{
    public class CreateSiteSettingsService : ICreateSiteSettingsService
    {
        private readonly ISiteSettingRepository _siteSettingRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSiteSettingsService(
            IUnitOfWork unitOfWork,
            ISiteSettingRepository siteSettingRepository)
        {
            _unitOfWork = unitOfWork;
            _siteSettingRepository = siteSettingRepository;
        }

        public async Task<SiteSettingCreatedResponseDto> CreateSiteSettingsAsync(CreateSiteSettingDto dto, CancellationToken cancellationToken)
        {
            var settingsExist = await _siteSettingRepository.AnySettingsExistAsync(cancellationToken);
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

            _siteSettingRepository.Add(siteSetting);
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