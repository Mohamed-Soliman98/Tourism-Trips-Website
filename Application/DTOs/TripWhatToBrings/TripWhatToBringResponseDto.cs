namespace Application.DTOs.TripWhatToBrings
{
    public sealed record TripWhatToBringResponseDto(
        Guid Id,
        Guid TripId,
        string Description
    );
}