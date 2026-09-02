namespace Application.DTOs.TripExcludes
{
    public sealed record TripExcludeUpdatedResponseDto(
        Guid Id,
        Guid TripId,
        string Description,
        string Message = "Trip exclude updated successfully."
    );
}
