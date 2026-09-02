namespace Application.DTOs.TourTypes
{
    public sealed record TourTypeUpdatedResponseDto(
        Guid Id,
        string Name,
        bool IsActive,
        string Message = "Tour type updated successfully."
    );
}
