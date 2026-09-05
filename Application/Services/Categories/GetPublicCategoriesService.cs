using Application.DTOs.Categories;
using Application.Interfaces.Categories;
using Application.Interfaces.Repositories;

namespace Application.Services.Categories
{
    public class GetPublicCategoriesService : IGetPublicCategoriesService
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetPublicCategoriesService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<List<PublicCategoryDto>> GetPublicCategoriesAsync(CancellationToken cancellationToken = default)
        {
            var categories = await _categoryRepository.GetActiveAsync(cancellationToken);

            return categories.Select(c => new PublicCategoryDto(
                c.Id,
                c.Name
            )).ToList();
        }
    }
}
