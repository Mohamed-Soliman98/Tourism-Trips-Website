using Domain.Enum;

namespace Application.DTOs.BookingInquiries
{
    public sealed record UpdateBookingInquiryDto(
        BookingInquiryStatus Status,
        string? InternalNotes = null
    );
}
