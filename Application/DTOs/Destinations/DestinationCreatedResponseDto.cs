namespace Application.DTOs.Destinations
{
    public sealed record DestinationCreatedResponseDto(
        Guid Id,
        string Name,
        bool IsActive,
        string Message = "Destination created successfully."
    );
}