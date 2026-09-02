namespace Application.DTOs.Categories
{
    public sealed record CategoryCreatedResponseDto(
        Guid Id,
        string Name,
        bool IsActive,
        string Message = "Category created successfully."
    );
}
