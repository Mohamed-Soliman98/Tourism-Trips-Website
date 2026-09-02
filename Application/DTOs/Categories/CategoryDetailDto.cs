namespace Application.DTOs.Categories
{
    public sealed record CategoryDetailDto(
        Guid Id,
        string Name,
        bool IsActive,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );
}
