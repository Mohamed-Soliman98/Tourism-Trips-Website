using Domain.Enum;

namespace Application.DTOs.TripTranslations
{
    public sealed record TripTranslationAddedResponseDto(
        Guid Id,
        Guid TripId,
        Language Language,
        string Title,
        string ShortDescription,
        string LongDescription,
        string? MetaTitle,
        string? MetaDescription,
        string Message = "Trip translation added successfully."
    );
}
