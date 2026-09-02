namespace Application.DTOs.Banners
{
    public sealed record CreateBannerDto(
        string Title,
        string Description,
        string ImageUrl,
        string? ButtonText,
        string? ButtonUrl,
        int DisplayOrder,
        bool IsActive = true
    );
}