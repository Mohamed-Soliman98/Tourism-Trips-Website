namespace Application.DTOs.TripImages
{
    /// <summary>
    /// Request DTO for updating the metadata of an existing TripImage.
    /// ImageUrl / TripId are not editable — only presentation metadata.
    /// </summary>
    public sealed record UpdateTripImageDto(
        string? AltText,
        int DisplayOrder,
        bool IsCover
    );
}
