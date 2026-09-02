namespace Application.DTOs.Testimonials
{
    public sealed record TestimonialSummaryDto(
        Guid Id,
        string CustomerName,
        string? Country,
        int Rating,
        string Content,
        bool IsActive,
        DateTime CreatedAt
    );
}