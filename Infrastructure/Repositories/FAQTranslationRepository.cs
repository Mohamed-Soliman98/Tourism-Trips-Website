using Application.Interfaces.Repositories;
using Domain.Entitys;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class FAQTranslationRepository : RepositoryGeneric<FAQTranslation>, IFAQTranslationRepository
    {
        public FAQTranslationRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<List<FAQTranslation>> GetByFAQIdAsync(
            Guid faqId,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(translation => translation.FAQId == faqId)
                .ToListAsync(cancellationToken);
        }
    }
}
