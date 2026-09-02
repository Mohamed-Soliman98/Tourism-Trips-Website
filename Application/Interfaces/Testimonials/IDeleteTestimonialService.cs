using Application.DTOs.Testimonials;

namespace Application.Interfaces.Testimonials
{
    public interface IDeleteTestimonialService
    {
        Task<TestimonialDeletedResponseDto?> DeleteTestimonialAsync(Guid id, CancellationToken cancellationToken);
    }
}