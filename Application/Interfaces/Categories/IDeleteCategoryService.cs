using Application.DTOs.Categories;

namespace Application.Interfaces.Categories
{
    public interface IDeleteCategoryService
    {
        Task<CategoryDeletedResponseDto> DeleteCategoryAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}
