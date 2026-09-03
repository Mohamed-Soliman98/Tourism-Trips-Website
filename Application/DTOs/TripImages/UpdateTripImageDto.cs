namespace Application.DTOs.TripImages
{
    public sealed record UpdateTripImageDto(
        string? AltText,
        int DisplayOrder,
        bool IsCover
    );
}
