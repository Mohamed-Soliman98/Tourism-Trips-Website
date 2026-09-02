namespace Application.DTOs.TripExcludes
{
    public sealed record TripExcludeAddedResponseDto(
        Guid Id,
        Guid TripId,
        string Description,
        string Message = "Trip exclude added successfully."
    );
}
