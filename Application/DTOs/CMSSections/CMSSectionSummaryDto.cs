namespace Application.DTOs.CMSSections
{
    public sealed record CMSSectionSummaryDto(
        Guid Id,
        string Key,
        string Title,
        string Content,
        string? ImageUrl,
        int DisplayOrder,
        bool IsActive,
        DateTime CreatedAt
    );
}