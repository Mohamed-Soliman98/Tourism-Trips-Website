using Application.DTOs.Common;
using Application.DTOs.Testimonials;
using Domain.Entity;

namespace Application.Interfaces.Repositories
{
    public interface ITestimonialRepository : IRepositoryGeneric<Testimonial>
    {
        Task<PagedResult<TestimonialSummaryDto>> GetTestimonialsAsync(GetTestimonialsQueryDto query, CancellationToken cancellationToken);

        Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);

        Task<int> GetActiveCountAsync(CancellationToken cancellationToken = default);

        Task<int> GetInactiveCountAsync(CancellationToken cancellationToken = default);
    }
}