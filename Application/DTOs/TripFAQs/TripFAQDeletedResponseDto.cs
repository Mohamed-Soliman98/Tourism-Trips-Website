namespace Application.DTOs.TripFAQs
{
    public sealed record TripFAQDeletedResponseDto(
        Guid Id,
        string Message = "Trip FAQ deleted successfully."
    );
}