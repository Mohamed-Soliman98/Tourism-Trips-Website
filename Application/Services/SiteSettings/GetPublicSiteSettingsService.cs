using Application.DTOs.SiteSettings;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.SiteSettings;

namespace Application.Services.SiteSettings
{
    public class GetPublicSiteSettingsService : IGetPublicSiteSettingsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPublicSiteSettingsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PublicSiteSettingDto?> GetPublicSiteSettingsAsync(CancellationToken cancellationToken = default)
        {
            var settings = await _unitOfWork.SiteSettings.GetSiteSettingsAsync(cancellationToken);

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
