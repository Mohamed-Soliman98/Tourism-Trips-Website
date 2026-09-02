namespace Application.DTOs.Banners
{
    public sealed record GetBannersQueryDto(
        int Page = 1,
        int PageSize = 10,
        bool? IsActive = null,
        string? SearchTerm = null
    );
}