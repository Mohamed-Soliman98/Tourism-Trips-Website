using Domain.Enum;

namespace Application.DTOs.BookingInquiries
{
    public sealed record BookingInquirySummaryDto(
        Guid Id,
        Guid TripId,
        string TripTitle,
        string Name,
        string Email,
        string Phone,
        string? WhatsApp,
        DateTime SelectedDate,
        int Adults,
        int Children,
        BookingInquiryStatus Status,
        DateTime CreatedAt
    );
}
