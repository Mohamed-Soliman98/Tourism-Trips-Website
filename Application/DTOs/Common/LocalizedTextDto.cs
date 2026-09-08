namespace Application.DTOs.Common
{
    public sealed record LocalizedTextDto
    {
        public string? English { get; set; }
        public string? German { get; set; }
        public string? French { get; set; }
        public string? Russian { get; set; }
    }
}
