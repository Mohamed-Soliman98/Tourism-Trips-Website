namespace Application.DTOs.Banners
{
    public sealed record UpdateBannerDto(
        string Title,
        string Description,
        string ImageUrl,
        string? ButtonText,
        string? ButtonUrl,
        int DisplayOrder,
        bool IsActive
    );
}