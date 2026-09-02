using Application.DTOs.Categories;

namespace Application.Interfaces.Categories
{
    public interface IGetPublicCategoriesService
    {
        Task<List<PublicCategoryDto>> GetPublicCategoriesAsync(CancellationToken cancellationToken = default);
    }
}
