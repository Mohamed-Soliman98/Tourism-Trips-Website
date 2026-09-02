namespace Application.DTOs.TourTypes
{
    public sealed record TourTypeCreatedResponseDto(
        Guid Id,
        string Name,
        bool IsActive,
        string Message = "Tour type created successfully."
    );
}
