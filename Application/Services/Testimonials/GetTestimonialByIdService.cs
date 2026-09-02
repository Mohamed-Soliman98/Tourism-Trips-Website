using Application.DTOs.Testimonials;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Testimonials;

namespace Application.Services.Testimonials
{
    public class GetTestimonialByIdService : IGetTestimonialByIdService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTestimonialByIdService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TestimonialDetailDto?> GetTestimonialByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var testimonial = await _unitOfWork.Testimonials.GetByIdAsync(id, cancellationToken);
            
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