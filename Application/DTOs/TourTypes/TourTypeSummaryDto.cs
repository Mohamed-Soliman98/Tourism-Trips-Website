namespace Application.DTOs.TourTypes
{
    public sealed record TourTypeSummaryDto(
        Guid Id,
        string Name,
        bool IsActive
    );
}
