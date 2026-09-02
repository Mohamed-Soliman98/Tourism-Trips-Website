using Domain.Entitys;

namespace Application.Interfaces.Repositories
{
    public interface IFAQTranslationRepository : IRepositoryGeneric<FAQTranslation>
    {
        Task<List<FAQTranslation>> GetByFAQIdAsync(
            Guid faqId,
            CancellationToken cancellationToken = default);
    }
}
