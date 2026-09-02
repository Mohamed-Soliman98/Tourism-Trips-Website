namespace Application.DTOs.Testimonials
{
    public sealed record TestimonialCreatedResponseDto(
        Guid Id,
        string CustomerName,
        string? Country,
        int Rating,
        string Content,
        bool IsActive,
        DateTime CreatedAt
    );
}