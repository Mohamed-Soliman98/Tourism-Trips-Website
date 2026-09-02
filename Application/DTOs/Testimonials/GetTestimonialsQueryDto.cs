namespace Application.DTOs.Testimonials
{
    public sealed record GetTestimonialsQueryDto(
        int Page = 1,
        int PageSize = 10,
        bool? IsActive = null,
        string? SearchTerm = null
    );
}