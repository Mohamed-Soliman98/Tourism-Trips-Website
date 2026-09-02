namespace Application.DTOs.Categories
{
    public sealed record GetCategoriesQueryDto(
        string? Search = null,
        bool? IsActive = null,
        int Page = 1,
        int PageSize = 10
    );
}
