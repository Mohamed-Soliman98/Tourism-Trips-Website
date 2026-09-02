using System;

namespace Domain.Entity
{
    public class SiteSetting
    {
        public Guid Id { get; set; }

        public string CompanyName { get; set; } = string.Empty;

        public string Phone { get; set; } = string.Empty;

        public string? WhatsApp { get; set; }

        public string Email { get; set; } = string.Empty;

        public string Address { get; set; } = string.Empty;

        public string? FacebookUrl { get; set; }

        public string? InstagramUrl { get; set; }

        public string? YouTubeUrl { get; set; }

        public string? TikTokUrl { get; set; }

        public string DefaultMetaTitle { get; set; } = string.Empty;

        public string DefaultMetaDescription { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}