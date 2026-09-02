namespace Application.DTOs.Destinations
{
    public sealed record CreateDestinationDto(
        string Name,
        bool IsActive = true
    );
}