using Domain.Enum;

namespace Application.DTOs.BookingInquiries
{
    public sealed record BookingInquiryDetailDto(
        Guid Id,
        Guid TripId,
        string TripTitle,
        string TripSlug,
        string Name,
        string Email,
        string Phone,
        string? WhatsApp,
        string? Nationality,
        string? HotelOrPickup,
        DateTime SelectedDate,
        int Adults,
        int Children,
        string? Notes,
        BookingInquiryStatus Status,
        string? InternalNotes,
        DateTime CreatedAt,
        DateTime? UpdatedAt
    );
}
