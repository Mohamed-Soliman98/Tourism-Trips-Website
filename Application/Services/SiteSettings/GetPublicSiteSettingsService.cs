using Application.DTOs.SiteSettings;
using Application.Interfaces.Repositories;
using Application.Interfaces.SiteSettings;

namespace Application.Services.SiteSettings
{
    public class GetPublicSiteSettingsService : IGetPublicSiteSettingsService
    {
        private readonly ISiteSettingRepository _siteSettingRepository;

        public GetPublicSiteSettingsService(ISiteSettingRepository siteSettingRepository)
        {
            _siteSettingRepository = siteSettingRepository;
        }

        public async Task<PublicSiteSettingDto?> GetPublicSiteSettingsAsync(CancellationToken cancellationToken = default)
        {
            var settings = await _siteSettingRepository.GetSiteSettingsAsync(cancellationToken);

            if (settings == null)
            {
                return null;
            }

            return new PublicSiteSettingDto(
                settings.CompanyName,
                settings.Phone,
                settings.WhatsApp,
                settings.Email,
                settings.Address,
                settings.FacebookUrl,
                settings.InstagramUrl,
                settings.YouTubeUrl,
                settings.TikTokUrl,
                settings.DefaultMetaTitle,
                settings.DefaultMetaDescription
            );
        }
    }
}
