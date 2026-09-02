using Application.DTOs.BookingInquiries;

namespace Application.Interfaces.BookingInquiries
{
    public interface IDeleteBookingInquiryService
    {
        Task<BookingInquiryDeletedResponseDto> DeleteBookingInquiryAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}
