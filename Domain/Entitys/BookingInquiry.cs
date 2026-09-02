using Domain.Enum;
using System;

namespace Domain.Entity
{
    public class BookingInquiry
    {
        public Guid Id { get; set; }

        public Guid TripId { get; set; }
        public Trip Trip { get; set; } = null!;

        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string? WhatsApp { get; set; }

        public string? Nationality { get; set; }
        public string? HotelOrPickup { get; set; }

        public DateTime SelectedDate { get; set; }
        public int Adults { get; set; } = 1;
        public int Children { get; set; } = 0;

        public string? Notes { get; set; }

        public BookingInquiryStatus Status { get; set; } = BookingInquiryStatus.New;
        public string? InternalNotes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
