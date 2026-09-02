using Application.DTOs.SiteSettings;

namespace Application.Interfaces.SiteSettings
{
    public interface IGetPublicSiteSettingsService
    {
        Task<PublicSiteSettingDto?> GetPublicSiteSettingsAsync(CancellationToken cancellationToken = default);
    }
}
