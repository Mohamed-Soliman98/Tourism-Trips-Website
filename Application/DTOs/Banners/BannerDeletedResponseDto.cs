namespace Application.DTOs.Banners
{
    public sealed record BannerDeletedResponseDto(
        Guid Id,
        string Message
    );
}