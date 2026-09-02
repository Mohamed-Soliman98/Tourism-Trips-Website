namespace Application.DTOs.CMSSections
{
    public sealed record UpdateCMSSectionDto(
        string Key,
        string Title,
        string Content,
        string? ImageUrl,
        int DisplayOrder,
        bool IsActive
    );
}