namespace Application.DTOs.TourTypes
{
    public sealed record TourTypeDetailDto(
        Guid Id,
        string Name,
        bool IsActive,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );
}
