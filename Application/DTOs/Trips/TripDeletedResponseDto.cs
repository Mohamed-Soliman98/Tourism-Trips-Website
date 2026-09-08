namespace Application.DTOs.Trips
{
    public sealed record TripDeletedResponseDto
    (
        string Message = "Trip deleted successfully."
    );
}
