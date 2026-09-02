using Domain.Enum;

namespace Application.DTOs.BookingInquiries
{
    public sealed record BookingInquiryUpdatedResponseDto(
        Guid Id,
        BookingInquiryStatus Status,
        string Message = "Booking inquiry updated successfully."
    );
}
