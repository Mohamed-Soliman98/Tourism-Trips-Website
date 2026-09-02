using Domain.Enum;

namespace Application.DTOs.BookingInquiries
{
    public sealed record GetBookingInquiriesQueryDto(
        Guid? TripId = null,
        BookingInquiryStatus? Status = null,
        DateTime? FromDate = null,
        DateTime? ToDate = null,
        string? Search = null,
        int Page = 1,
        int PageSize = 10
    );
}
