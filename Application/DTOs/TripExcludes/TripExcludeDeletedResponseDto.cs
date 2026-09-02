namespace Application.DTOs.TripExcludes
{
    public sealed record TripExcludeDeletedResponseDto(
        string Message = "Trip exclude deleted successfully."
    );
}
