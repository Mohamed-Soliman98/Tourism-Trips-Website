using Application.DTOs.Categories;

namespace Application.Interfaces.Categories
{
    public interface ICreateCategoryService
    {
        Task<CategoryCreatedResponseDto> CreateCategoryAsync(CreateCategoryDto dto, CancellationToken cancellationToken = default);
    }
}
