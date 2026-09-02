namespace Application.DTOs.TourTypes
{
    public sealed record GetTourTypesQueryDto(
        string? Search = null,
        bool? IsActive = null,
        int Page = 1,
        int PageSize = 10
    );
}
