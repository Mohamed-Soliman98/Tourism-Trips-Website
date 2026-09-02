using Application.DTOs.Categories;

namespace Application.Interfaces.Categories
{
    public interface IUpdateCategoryService
    {
        Task<CategoryUpdatedResponseDto> UpdateCategoryAsync(
            Guid id,
            UpdateCategoryDto dto,
            CancellationToken cancellationToken = default);
    }
}
