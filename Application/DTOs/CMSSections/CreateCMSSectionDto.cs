namespace Application.DTOs.CMSSections
{
    public sealed record CreateCMSSectionDto(
        string Key,
        string Title,
        string Content,
        string? ImageUrl,
        int DisplayOrder,
        bool IsActive = true
    );
}