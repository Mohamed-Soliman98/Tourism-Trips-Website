namespace Application.DTOs.Banners
{
    public sealed record PublicBannerDto(
        Guid Id,
        string Title,
        string Description,
        string ImageUrl,
        string? ButtonText,
        string? ButtonUrl,
        int DisplayOrder
    );
}
