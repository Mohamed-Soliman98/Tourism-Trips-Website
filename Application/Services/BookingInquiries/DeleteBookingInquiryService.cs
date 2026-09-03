using Application.DTOs.BookingInquiries;
using Application.Interfaces.BookingInquiries;
using Application.Interfaces.IUnitOfWork;

namespace Application.Services.BookingInquiries
{
    public class DeleteBookingInquiryService : IDeleteBookingInquiryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBookingInquiryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BookingInquiryDeletedResponseDto> DeleteBookingInquiryAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Booking inquiry Id cannot be empty.", nameof(id));
            }

            var inquiry = await _unitOfWork.BookingInquiries.GetByIdAsync(id, cancellationToken);

            if (inquiry == null)
            {
                throw new KeyNotFoundException($"Booking inquiry with ID '{id}' was not found.");
            }

            _unitOfWork.BookingInquiries.Remove(inquiry);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new BookingInquiryDeletedResponseDto("Booking inquiry deleted successfully.");
        }
    }
}
