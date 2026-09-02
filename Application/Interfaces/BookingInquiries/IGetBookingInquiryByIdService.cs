using Application.DTOs.BookingInquiries;

namespace Application.Interfaces.BookingInquiries
{
    public interface IGetBookingInquiryByIdService
    {
        Task<BookingInquiryDetailDto> GetBookingInquiryByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}
