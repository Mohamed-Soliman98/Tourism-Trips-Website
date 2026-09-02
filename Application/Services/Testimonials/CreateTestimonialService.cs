using Application.DTOs.Testimonials;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Testimonials;
using Domain.Entity;

namespace Application.Services.Testimonials
{
    public class CreateTestimonialService : ICreateTestimonialService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateTestimonialService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TestimonialCreatedResponseDto> CreateTestimonialAsync(CreateTestimonialDto dto, CancellationToken cancellationToken)
        {
            var testimonial = new Testimonial
            {
                Id = Guid.NewGuid(),
                CustomerName = dto.CustomerName,
                Country = dto.Country,
                Rating = dto.Rating,
                Content = dto.Content,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            _unitOfWork.Testimonials.Add(testimonial);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TestimonialCreatedResponseDto(
                testimonial.Id,
                testimonial.CustomerName,
                testimonial.Country,
                testimonial.Rating,
                testimonial.Content,
                testimonial.IsActive,
                testimonial.CreatedAt
            );
        }
    }
}