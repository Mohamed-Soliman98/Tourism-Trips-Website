namespace Application.DTOs.Destinations
{
    public sealed record DestinationDeletedResponseDto(
        string Message = "Destination deleted successfully."
    );
}
