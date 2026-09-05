using Application.DTOs.Testimonials;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.Testimonials;

namespace Application.Services.Testimonials
{
    public class DeleteTestimonialService : IDeleteTestimonialService
    {
        private readonly ITestimonialRepository _testimonialRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTestimonialService(
            IUnitOfWork unitOfWork,
            ITestimonialRepository testimonialRepository)
        {
            _unitOfWork = unitOfWork;
            _testimonialRepository = testimonialRepository;
        }

        public async Task<TestimonialDeletedResponseDto?> DeleteTestimonialAsync(Guid id, CancellationToken cancellationToken)
        {
            var testimonial = await _testimonialRepository.GetByIdAsync(id, cancellationToken);
            
            if (testimonial == null)
                return null;

            _testimonialRepository.Remove(testimonial);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TestimonialDeletedResponseDto(
                id,
                "Testimonial deleted successfully."
            );
        }
    }
}