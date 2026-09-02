using Application.DTOs.Testimonials;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Testimonials;

namespace Application.Services.Testimonials
{
    public class UpdateTestimonialService : IUpdateTestimonialService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateTestimonialService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TestimonialUpdatedResponseDto?> UpdateTestimonialAsync(Guid id, UpdateTestimonialDto dto, CancellationToken cancellationToken)
        {
            var testimonial = await _unitOfWork.Testimonials.GetByIdAsync(id, cancellationToken);
            
            if (testimonial == null)
                return null;

            testimonial.CustomerName = dto.CustomerName;
            testimonial.Country = dto.Country;
            testimonial.Rating = dto.Rating;
            testimonial.Content = dto.Content;
            testimonial.IsActive = dto.IsActive;
            testimonial.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Testimonials.Update(testimonial);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TestimonialUpdatedResponseDto(
                testimonial.Id,
                testimonial.CustomerName,
                testimonial.Country,
                testimonial.Rating,
                testimonial.Content,
                testimonial.IsActive,
                testimonial.UpdatedAt.Value
            );
        }
    }
}