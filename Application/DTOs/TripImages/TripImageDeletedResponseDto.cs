namespace Application.DTOs.TripImages
{
    public sealed record TripImageDeletedResponseDto(
        string Message = "Image removed from gallery successfully."
    );
}
