using Application.DTOs.Common;
using Application.DTOs.Testimonials;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Testimonials;

namespace Application.Services.Testimonials
{
    public class GetTestimonialsService : IGetTestimonialsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTestimonialsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<TestimonialSummaryDto>> GetTestimonialsAsync(GetTestimonialsQueryDto query, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Testimonials.GetTestimonialsAsync(query, cancellationToken);
        }
    }
}