namespace Application.DTOs.Banners
{
    public sealed record BannerCreatedResponseDto(
        Guid Id,
        string Title,
        string Description,
        string ImageUrl,
        string? ButtonText,
        string? ButtonUrl,
        int DisplayOrder,
        bool IsActive,
        DateTime CreatedAt
    );
}