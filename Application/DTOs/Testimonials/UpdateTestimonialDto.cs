namespace Application.DTOs.Testimonials
{
    public sealed record UpdateTestimonialDto(
        string CustomerName,
        string? Country,
        int Rating,
        string Content,
        bool IsActive
    );
}