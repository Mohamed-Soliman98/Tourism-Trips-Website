namespace Application.DTOs.TripTranslations
{
    public sealed record TripTranslationDeletedResponseDto(
        string Message = "Trip translation deleted successfully."
    );
}
