namespace Application.DTOs.Categories
{
    public sealed record CategoryDeletedResponseDto(
        string Message = "Category deleted successfully."
    );
}
