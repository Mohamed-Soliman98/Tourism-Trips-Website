using Domain.Enum;

namespace Application.DTOs.TripTranslations
{
    public sealed record UpdateTripTranslationDto
    {
        public Language Language { get; set; }
        public string Title { get; set; } = string.Empty;
        public string ShortDescription { get; set; } = string.Empty;
        public string LongDescription { get; set; } = string.Empty;
        public string? MetaTitle { get; set; }
        public string? MetaDescription { get; set; }
        public string? PickupLocation { get; set; }
    }
}
