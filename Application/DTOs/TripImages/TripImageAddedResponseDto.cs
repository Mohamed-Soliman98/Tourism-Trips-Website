namespace Application.DTOs.TripImages
{
    public sealed record TripImageAddedResponseDto(
        Guid Id,
        Guid TripId,
        string ImageUrl,
        string? AltText,
        int DisplayOrder,
        bool IsCover,
        string Message = "Image added to gallery successfully."
    );
}
