using Application.DTOs.SiteSettings;

namespace Application.Interfaces.SiteSettings
{
    public interface ICreateSiteSettingsService
    {
        Task<SiteSettingCreatedResponseDto> CreateSiteSettingsAsync(CreateSiteSettingDto dto, CancellationToken cancellationToken);
    }
}