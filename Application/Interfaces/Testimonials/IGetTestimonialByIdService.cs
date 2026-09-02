using Application.DTOs.Testimonials;

namespace Application.Interfaces.Testimonials
{
    public interface IGetTestimonialByIdService
    {
        Task<TestimonialDetailDto?> GetTestimonialByIdAsync(Guid id, CancellationToken cancellationToken);
    }
}