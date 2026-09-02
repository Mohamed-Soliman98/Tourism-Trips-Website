using Application.DTOs.Testimonials;

namespace Application.Interfaces.Testimonials
{
    public interface IUpdateTestimonialService
    {
        Task<TestimonialUpdatedResponseDto?> UpdateTestimonialAsync(Guid id, UpdateTestimonialDto dto, CancellationToken cancellationToken);
    }
}