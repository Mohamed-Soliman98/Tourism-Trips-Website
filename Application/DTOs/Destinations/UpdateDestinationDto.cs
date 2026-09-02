namespace Application.DTOs.Destinations
{
    public sealed record UpdateDestinationDto(
        string Name,
        bool IsActive
    );
}
