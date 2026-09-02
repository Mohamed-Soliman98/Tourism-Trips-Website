using Application.DTOs.Categories;
using Application.Interfaces.Categories;
using Application.Interfaces.IUnitOfWork;

namespace Application.Services.Categories
{
    public class GetPublicCategoriesService : IGetPublicCategoriesService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPublicCategoriesService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PublicCategoryDto>> GetPublicCategoriesAsync(CancellationToken cancellationToken = default)
        {
            var categories = await _unitOfWork.Categories.GetActiveAsync(cancellationToken);

            return categories.Select(c => new PublicCategoryDto(
                c.Id,
                c.Name
            )).ToList();
        }
    }
}
