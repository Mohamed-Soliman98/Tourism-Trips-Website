namespace Application.DTOs.Destinations
{
    public sealed record DestinationSummaryDto(
        Guid Id,
        string Name,
        bool IsActive
    );
}
