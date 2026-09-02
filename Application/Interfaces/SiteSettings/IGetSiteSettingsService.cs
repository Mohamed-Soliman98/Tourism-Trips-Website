using Application.DTOs.SiteSettings;

namespace Application.Interfaces.SiteSettings
{
    public interface IGetSiteSettingsService
    {
        Task<SiteSettingDto?> GetSiteSettingsAsync(CancellationToken cancellationToken);
    }
}