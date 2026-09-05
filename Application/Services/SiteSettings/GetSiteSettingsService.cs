using Application.DTOs.SiteSettings;
using Application.Interfaces.Repositories;
using Application.Interfaces.SiteSettings;

namespace Application.Services.SiteSettings
{
    public class GetSiteSettingsService : IGetSiteSettingsService
    {
        private readonly ISiteSettingRepository _siteSettingRepository;

        public GetSiteSettingsService(ISiteSettingRepository siteSettingRepository)
        {
            _siteSettingRepository = siteSettingRepository;
        }

        public async Task<SiteSettingDto?> GetSiteSettingsAsync(CancellationToken cancellationToken)
        {
            var siteSetting = await _siteSettingRepository.GetSiteSettingsAsync(cancellationToken);
            
            if (siteSetting == null)
                return null;

            return new SiteSettingDto(
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
                siteSetting.CreatedAt,
                siteSetting.UpdatedAt
            );
        }
    }
}