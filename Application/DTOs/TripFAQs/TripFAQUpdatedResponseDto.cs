namespace Application.DTOs.TripFAQs
{
    public sealed record TripFAQUpdatedResponseDto(
        Guid Id,
        Guid TripId,
        string Question,
        string Answer,
        int DisplayOrder,
        bool IsActive,
        string Message = "Trip FAQ updated successfully."
    );
}