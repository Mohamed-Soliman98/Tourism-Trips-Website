namespace Application.DTOs.TripExcludes
{
    public sealed record TripExcludeResponseDto(
        Guid Id,
        Guid TripId,
        string Description
    );
}
