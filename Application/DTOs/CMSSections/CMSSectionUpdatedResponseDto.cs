namespace Application.DTOs.CMSSections
{
    public sealed record CMSSectionUpdatedResponseDto(
        Guid Id,
        string Key,
        string Title,
        string Content,
        string? ImageUrl,
        int DisplayOrder,
        bool IsActive,
        DateTime UpdatedAt
    );
}