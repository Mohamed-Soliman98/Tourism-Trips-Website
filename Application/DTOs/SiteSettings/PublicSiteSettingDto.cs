namespace Application.DTOs.SiteSettings
{
    /// <summary>
    /// Public site settings exposed to the website.
    /// Does NOT include sensitive/internal/admin fields.
    /// </summary>
    public sealed record PublicSiteSettingDto(
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
