using Application.DTOs.Categories;
using Application.Interfaces.Categories;
using Application.Interfaces.Repositories;

namespace Application.Services.Categories
{
    public class GetCategoryByIdService : IGetCategoryByIdService
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryByIdService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDetailDto> GetCategoryByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Category Id cannot be empty.", nameof(id));
            }

            var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID '{id}' was not found.");
            }

            return new CategoryDetailDto(
                category.Id,
                category.Name,
                category.IsActive,
                category.CreatedAt,
                category.UpdatedAt
            );
        }
    }
}
