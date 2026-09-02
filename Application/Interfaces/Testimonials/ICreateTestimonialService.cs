using Application.DTOs.Testimonials;

namespace Application.Interfaces.Testimonials
{
    public interface ICreateTestimonialService
    {
        Task<TestimonialCreatedResponseDto> CreateTestimonialAsync(CreateTestimonialDto dto, CancellationToken cancellationToken);
    }
}