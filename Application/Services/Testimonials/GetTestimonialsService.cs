using Application.DTOs.Common;
using Application.DTOs.Testimonials;
using Application.Interfaces.Repositories;
using Application.Interfaces.Testimonials;

namespace Application.Services.Testimonials
{
    public class GetTestimonialsService : IGetTestimonialsService
    {
        private readonly ITestimonialRepository _testimonialRepository;

        public GetTestimonialsService(ITestimonialRepository testimonialRepository)
        {
            _testimonialRepository = testimonialRepository;
        }

        public async Task<PagedResult<TestimonialSummaryDto>> GetTestimonialsAsync(GetTestimonialsQueryDto query, CancellationToken cancellationToken)
        {
            return await _testimonialRepository.GetTestimonialsAsync(query, cancellationToken);
        }
    }
}