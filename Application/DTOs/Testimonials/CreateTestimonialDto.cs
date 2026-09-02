namespace Application.DTOs.Testimonials
{
    public sealed record CreateTestimonialDto(
        string CustomerName,
        string? Country,
        int Rating,
        string Content,
        bool IsActive = true
    );
}