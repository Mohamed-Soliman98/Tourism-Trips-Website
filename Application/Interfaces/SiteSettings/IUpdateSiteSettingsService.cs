using Application.DTOs.SiteSettings;

namespace Application.Interfaces.SiteSettings
{
    public interface IUpdateSiteSettingsService
    {
        Task<SiteSettingUpdatedResponseDto?> UpdateSiteSettingsAsync(UpdateSiteSettingDto dto, CancellationToken cancellationToken);
    }
}