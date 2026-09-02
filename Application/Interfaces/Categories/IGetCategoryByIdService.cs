using Application.DTOs.Categories;

namespace Application.Interfaces.Categories
{
    public interface IGetCategoryByIdService
    {
        Task<CategoryDetailDto> GetCategoryByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}
