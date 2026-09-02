using Application.DTOs.BookingInquiries;

namespace Application.Interfaces.BookingInquiries
{
    public interface ICreateBookingInquiryService
    {
        Task<BookingInquiryCreatedResponseDto> CreateBookingInquiryAsync(
            CreateBookingInquiryDto dto,
            CancellationToken cancellationToken = default);
    }
}
