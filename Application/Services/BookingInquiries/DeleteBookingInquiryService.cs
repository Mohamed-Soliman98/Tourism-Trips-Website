using Application.DTOs.BookingInquiries;
using Application.Interfaces.BookingInquiries;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;

namespace Application.Services.BookingInquiries
{
    public class DeleteBookingInquiryService : IDeleteBookingInquiryService
    {
        private readonly IBookingInquiryRepository _bookingInquiryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBookingInquiryService(
            IUnitOfWork unitOfWork,
            IBookingInquiryRepository bookingInquiryRepository)
        {
            _unitOfWork = unitOfWork;
            _bookingInquiryRepository = bookingInquiryRepository;
        }

        public async Task<BookingInquiryDeletedResponseDto> DeleteBookingInquiryAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Booking inquiry Id cannot be empty.", nameof(id));
            }

            var inquiry = await _bookingInquiryRepository.GetByIdAsync(id, cancellationToken);

            if (inquiry == null)
            {
                throw new KeyNotFoundException($"Booking inquiry with ID '{id}' was not found.");
            }

            _bookingInquiryRepository.Remove(inquiry);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new BookingInquiryDeletedResponseDto("Booking inquiry deleted successfully.");
        }
    }
}
