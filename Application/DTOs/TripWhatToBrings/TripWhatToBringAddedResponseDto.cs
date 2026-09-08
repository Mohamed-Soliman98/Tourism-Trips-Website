namespace Application.DTOs.TripWhatToBrings
{
    public sealed record TripWhatToBringAddedResponseDto(
        Guid Id,
        Guid TripId,
        string Description,
        string Message = "Trip what-to-bring item added successfully."
    );
}