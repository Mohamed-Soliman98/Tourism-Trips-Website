namespace Application.DTOs.TripIncludes
{
    public sealed record TripIncludeResponseDto(
        Guid Id,
        Guid TripId,
        string Description
    );
}
