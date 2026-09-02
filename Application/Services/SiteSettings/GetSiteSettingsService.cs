using Application.DTOs.SiteSettings;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.SiteSettings;

namespace Application.Services.SiteSettings
{
    public class GetSiteSettingsService : IGetSiteSettingsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetSiteSettingsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<SiteSettingDto?> GetSiteSettingsAsync(CancellationToken cancellationToken)
        {
            var siteSetting = await _unitOfWork.SiteSettings.GetSiteSettingsAsync(cancellationToken);
            
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