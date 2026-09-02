namespace Application.DTOs.Trips
{
    public sealed record TripCreatedResponseDto(
        Guid TripId,
        string Slug,
        string Message = "Trip created successfully as Draft."
    );
}
