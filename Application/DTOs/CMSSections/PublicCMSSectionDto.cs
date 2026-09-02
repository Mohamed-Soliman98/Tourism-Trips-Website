namespace Application.DTOs.CMSSections
{
    public sealed record PublicCMSSectionDto(
        Guid Id,
        string Key,
        string Title,
        string Content,
        string? ImageUrl,
        int DisplayOrder
    );
}
