namespace Application.DTOs.TripFAQs
{
    public sealed record TripFAQResponseDto(
        Guid Id,
        Guid TripId,
        string Question,
        string Answer,
        int DisplayOrder,
        bool IsActive
    );
}