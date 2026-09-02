namespace Application.DTOs.Categories
{
    public sealed record CategorySummaryDto(
        Guid Id,
        string Name,
        bool IsActive
    );
}
