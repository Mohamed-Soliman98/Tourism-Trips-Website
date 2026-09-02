namespace Application.DTOs.Trips
{
    public sealed record TripUpdatedResponseDto(
        Guid TripId,
        string Slug,
        string Message = "Trip updated successfully."
    );
}
