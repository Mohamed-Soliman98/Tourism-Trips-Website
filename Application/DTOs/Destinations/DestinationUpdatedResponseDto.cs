namespace Application.DTOs.Destinations
{
    public sealed record DestinationUpdatedResponseDto(
        Guid Id,
        string Name,
        bool IsActive,
        string Message = "Destination updated successfully."
    );
}
