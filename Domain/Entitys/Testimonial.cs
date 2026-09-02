using System;

namespace Domain.Entity
{
    public class Testimonial
    {
        public Guid Id { get; set; }

        public string CustomerName { get; set; } = string.Empty;

        public string? Country { get; set; }

        public int Rating { get; set; }

        public string Content { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? UpdatedAt { get; set; }
    }
}