namespace Application.DTOs.Destinations
{
    public sealed record GetDestinationsQueryDto(
        string? Search = null,
        bool? IsActive = null,
        int Page = 1,
        int PageSize = 10
    );
}
