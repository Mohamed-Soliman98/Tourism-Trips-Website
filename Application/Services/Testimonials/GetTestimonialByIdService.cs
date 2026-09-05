using Application.DTOs.Testimonials;
using Application.Interfaces.Repositories;
using Application.Interfaces.Testimonials;

namespace Application.Services.Testimonials
{
    public class GetTestimonialByIdService : IGetTestimonialByIdService
    {
        private readonly ITestimonialRepository _testimonialRepository;

        public GetTestimonialByIdService(ITestimonialRepository testimonialRepository)
        {
            _testimonialRepository = testimonialRepository;
        }

        public async Task<TestimonialDetailDto?> GetTestimonialByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var testimonial = await _testimonialRepository.GetByIdAsync(id, cancellationToken);
            
            if (testimonial == null)
                return null;

            return new TestimonialDetailDto(
                testimonial.Id,
                testimonial.CustomerName,
                testimonial.Country,
                testimonial.Rating,
                testimonial.Content,
                testimonial.IsActive,
                testimonial.CreatedAt,
                testimonial.UpdatedAt
            );
        }
    }
}