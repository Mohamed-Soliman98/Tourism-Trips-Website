namespace Application.DTOs.CMSSections
{
    public sealed record CMSSectionDetailDto(
        Guid Id,
        string Key,
        string Title,
        string Content,
        string? ImageUrl,
        int DisplayOrder,
        bool IsActive,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );
}