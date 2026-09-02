namespace Application.DTOs.CMSSections
{
    public sealed record GetCMSSectionsQueryDto(
        int Page = 1,
        int PageSize = 10,
        bool? IsActive = null,
        string? SearchTerm = null
    );
}