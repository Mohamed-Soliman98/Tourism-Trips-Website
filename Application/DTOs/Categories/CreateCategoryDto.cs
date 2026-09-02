namespace Application.DTOs.Categories
{
    public sealed record CreateCategoryDto(
        string Name,
        bool IsActive = true
    );
}
