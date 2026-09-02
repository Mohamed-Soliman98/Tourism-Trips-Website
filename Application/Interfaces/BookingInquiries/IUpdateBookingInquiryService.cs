using Application.DTOs.BookingInquiries;

namespace Application.Interfaces.BookingInquiries
{
    public interface IUpdateBookingInquiryService
    {
        Task<BookingInquiryUpdatedResponseDto> UpdateBookingInquiryAsync(
            Guid id,
            UpdateBookingInquiryDto dto,
            CancellationToken cancellationToken = default);
    }
}
