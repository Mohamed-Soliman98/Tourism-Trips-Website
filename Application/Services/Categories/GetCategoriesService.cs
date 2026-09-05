using Application.DTOs.Categories;
using Application.DTOs.Common;
using Application.Interfaces.Categories;
using Application.Interfaces.Repositories;
using Domain.Entity;
using FluentValidation;

namespace Application.Services.Categories
{
    public class GetCategoriesService : IGetCategoriesService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IValidator<GetCategoriesQueryDto> _validator;

        public GetCategoriesService(
            ICategoryRepository categoryRepository,
            IValidator<GetCategoriesQueryDto> validator)
        {
            _categoryRepository = categoryRepository;
            _validator = validator;
        }

        public async Task<PagedResult<CategorySummaryDto>> GetCategoriesAsync(
            GetCategoriesQueryDto query,
            CancellationToken cancellationToken = default)
        {
            await _validator.ValidateAndThrowAsync(query, cancellationToken);

            var (items, totalCount) = await _categoryRepository.GetCategoriesAsync(query, cancellationToken);

            var dtos = items.Select(MapToDto).ToList();

            return new PagedResult<CategorySummaryDto>(
                dtos,
                query.Page,
                query.PageSize,
                totalCount
            );
        }

        private static CategorySummaryDto MapToDto(Category category)
        {
            return new CategorySummaryDto(
                category.Id,
                category.Name,
                category.IsActive
            );
        }
    }
}
