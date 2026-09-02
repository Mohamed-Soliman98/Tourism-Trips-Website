namespace Application.DTOs.Testimonials
{
    public sealed record TestimonialUpdatedResponseDto(
        Guid Id,
        string CustomerName,
        string? Country,
        int Rating,
        string Content,
        bool IsActive,
        DateTime UpdatedAt
    );
}