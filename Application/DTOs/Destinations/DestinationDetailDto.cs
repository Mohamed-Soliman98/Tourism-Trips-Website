namespace Application.DTOs.Destinations
{
    public sealed record DestinationDetailDto(
        Guid Id,
        string Name,
        bool IsActive,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );
}
