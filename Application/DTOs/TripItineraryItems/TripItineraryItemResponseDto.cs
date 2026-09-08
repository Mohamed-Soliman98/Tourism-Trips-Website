namespace Application.DTOs.TripItineraryItems
{
    public sealed record TripItineraryItemResponseDto(
        Guid Id,
        Guid TripId,
        int DisplayOrder,
        string Title,
        string? Description
    );
}