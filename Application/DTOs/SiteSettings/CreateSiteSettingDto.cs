namespace Application.DTOs.SiteSettings
{
    public sealed record CreateSiteSettingDto(
        string CompanyName,
        string Phone,
        string? WhatsApp,
        string Email,
        string Address,
        string? FacebookUrl,
        string? InstagramUrl,
        string? YouTubeUrl,
        string? TikTokUrl,
        string DefaultMetaTitle,
        string DefaultMetaDescription
    );
}