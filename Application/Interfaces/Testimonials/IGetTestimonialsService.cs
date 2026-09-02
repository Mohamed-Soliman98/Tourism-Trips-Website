using Application.DTOs.Common;
using Application.DTOs.Testimonials;

namespace Application.Interfaces.Testimonials
{
    public interface IGetTestimonialsService
    {
        Task<PagedResult<TestimonialSummaryDto>> GetTestimonialsAsync(GetTestimonialsQueryDto query, CancellationToken cancellationToken);
    }
}