namespace Application.DTOs.TripWhatToBrings
{
    public sealed record TripWhatToBringDeletedResponseDto(
        string Message = "Trip what-to-bring item deleted successfully."
    );
}