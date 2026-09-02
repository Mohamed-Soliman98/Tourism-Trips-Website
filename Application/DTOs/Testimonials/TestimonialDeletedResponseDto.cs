namespace Application.DTOs.Testimonials
{
    public sealed record TestimonialDeletedResponseDto(
        Guid Id,
        string Message
    );
}