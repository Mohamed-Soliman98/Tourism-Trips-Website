using Application.DTOs.BookingInquiries;
using Application.DTOs.Common;

namespace Application.Interfaces.BookingInquiries
{
    public interface IGetBookingInquiriesService
    {
        Task<PagedResult<BookingInquirySummaryDto>> GetBookingInquiriesAsync(
            GetBookingInquiriesQueryDto query,
            CancellationToken cancellationToken = default);
    }
}
