using Domain.Entity;

namespace Application.Interfaces.Repositories
{
    public interface ISiteSettingRepository : IRepositoryGeneric<SiteSetting>
    {
        Task<SiteSetting?> GetSiteSettingsAsync(CancellationToken cancellationToken);
        Task<bool> AnySettingsExistAsync(CancellationToken cancellationToken);
    }
}