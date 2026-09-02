using Application.DTOs.Testimonials;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Testimonials;

namespace Application.Services.Testimonials
{
    public class DeleteTestimonialService : IDeleteTestimonialService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTestimonialService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TestimonialDeletedResponseDto?> DeleteTestimonialAsync(Guid id, CancellationToken cancellationToken)
        {
            var testimonial = await _unitOfWork.Testimonials.GetByIdAsync(id, cancellationToken);
            
            if (testimonial == null)
                return null;

            _unitOfWork.Testimonials.Remove(testimonial);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TestimonialDeletedResponseDto(
                id,
                "Testimonial deleted successfully."
            );
        }
    }
}