namespace Application.DTOs.Categories
{
    public sealed record UpdateCategoryDto(
        string Name,
        bool IsActive
    );
}
