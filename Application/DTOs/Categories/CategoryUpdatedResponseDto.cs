namespace Application.DTOs.Categories
{
    public sealed record CategoryUpdatedResponseDto(
        Guid Id,
        string Name,
        bool IsActive,
        string Message = "Category updated successfully."
    );
}
