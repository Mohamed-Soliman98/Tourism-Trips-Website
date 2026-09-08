namespace Application.DTOs.TripWhatToBrings
{
    public sealed record TripWhatToBringUpdatedResponseDto(
        Guid Id,
        Guid TripId,
        string Description,
        string Message = "Trip what-to-bring item updated successfully."
    );
}