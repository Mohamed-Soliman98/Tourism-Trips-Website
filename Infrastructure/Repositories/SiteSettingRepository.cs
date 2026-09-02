using Application.Interfaces.Repositories;
using Domain.Entity;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class SiteSettingRepository : RepositoryGeneric<SiteSetting>, ISiteSettingRepository
    {
        public SiteSettingRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<SiteSetting?> GetSiteSettingsAsync(CancellationToken cancellationToken)
        {
            // Since we expect only one settings record, get the first one
            return await _dbSet.FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> AnySettingsExistAsync(CancellationToken cancellationToken)
        {
            return await _dbSet.AnyAsync(cancellationToken);
        }
    }
}