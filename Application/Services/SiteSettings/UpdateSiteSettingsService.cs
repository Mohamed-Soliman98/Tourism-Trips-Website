using Application.DTOs.SiteSettings;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.SiteSettings;

namespace Application.Services.SiteSettings
{
    public class UpdateSiteSettingsService : IUpdateSiteSettingsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateSiteSettingsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SiteSettingUpdatedResponseDto?> UpdateSiteSettingsAsync(UpdateSiteSettingDto dto, CancellationToken cancellationToken)
        {
            var siteSetting = await _unitOfWork.SiteSettings.GetSiteSettingsAsync(cancellationToken);
            
            if (siteSetting == null)
                return null;

            siteSetting.CompanyName = dto.CompanyName;
            siteSetting.Phone = dto.Phone;
            siteSetting.WhatsApp = dto.WhatsApp;
            siteSetting.Email = dto.Email;
            siteSetting.Address = dto.Address;
            siteSetting.FacebookUrl = dto.FacebookUrl;
            siteSetting.InstagramUrl = dto.InstagramUrl;
            siteSetting.YouTubeUrl = dto.YouTubeUrl;
            siteSetting.TikTokUrl = dto.TikTokUrl;
            siteSetting.DefaultMetaTitle = dto.DefaultMetaTitle;
            siteSetting.DefaultMetaDescription = dto.DefaultMetaDescription;
            siteSetting.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.SiteSettings.Update(siteSetting);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new SiteSettingUpdatedResponseDto(
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
                siteSetting.UpdatedAt.Value
            );
        }
    }
}