using Domain.Enum;

namespace Application.DTOs.Trips
{
    public sealed record CreateTripFAQDto
    {
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
        public int DisplayOrder { get; set; }
        public bool IsActive { get; set; } = true;
        public List<CreateFAQTranslationDto> Translations { get; set; } = new();
    }

    public sealed record CreateFAQTranslationDto
    {
        public Language Language { get; set; }
        public string Question { get; set; } = string.Empty;
        public string Answer { get; set; } = string.Empty;
    }
}
