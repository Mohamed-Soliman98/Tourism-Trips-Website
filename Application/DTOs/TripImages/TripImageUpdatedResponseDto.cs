namespace Application.DTOs.TripImages
{
    public sealed record TripImageUpdatedResponseDto(
        Guid Id,
        Guid TripId,
        string ImageUrl,
        string? AltText,
        int DisplayOrder,
        bool IsCover
    );
}
