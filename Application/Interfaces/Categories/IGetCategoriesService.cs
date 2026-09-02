using Application.DTOs.Categories;
using Application.DTOs.Common;

namespace Application.Interfaces.Categories
{
    public interface IGetCategoriesService
    {
        Task<PagedResult<CategorySummaryDto>> GetCategoriesAsync(
            GetCategoriesQueryDto query,
            CancellationToken cancellationToken = default);
    }
}
