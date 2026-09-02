namespace Application.DTOs.TripFAQs
{
    public sealed record TripFAQAddedResponseDto(
        Guid Id,
        Guid TripId,
        string Question,
        string Answer,
        int DisplayOrder,
        bool IsActive,
        string Message = "Trip FAQ added successfully."
    );
}