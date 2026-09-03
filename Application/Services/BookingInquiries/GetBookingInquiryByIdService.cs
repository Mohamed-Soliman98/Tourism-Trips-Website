using Application.DTOs.BookingInquiries;
using Application.Interfaces.BookingInquiries;
using Application.Interfaces.IUnitOfWork;

namespace Application.Services.BookingInquiries
{
    public class GetBookingInquiryByIdService : IGetBookingInquiryByIdService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBookingInquiryByIdService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BookingInquiryDetailDto> GetBookingInquiryByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Booking inquiry Id cannot be empty.", nameof(id));
            }

            var inquiry = await _unitOfWork.BookingInquiries.GetByIdWithTripAsync(id, cancellationToken);

            if (inquiry == null)
            {
                throw new KeyNotFoundException($"Booking inquiry with ID '{id}' was not found.");
            }

            return new BookingInquiryDetailDto(
                Id: inquiry.Id,
                TripId: inquiry.TripId,
                TripTitle: inquiry.Trip?.Title ?? string.Empty,
                TripSlug: inquiry.Trip?.Slug ?? string.Empty,
                Name: inquiry.Name,
                Email: inquiry.Email,
                Phone: inquiry.Phone,
                WhatsApp: inquiry.WhatsApp,
                Nationality: inquiry.Nationality,
                HotelOrPickup: inquiry.HotelOrPickup,
                SelectedDate: inquiry.SelectedDate,
                Adults: inquiry.Adults,
                Children: inquiry.Children,
                Notes: inquiry.Notes,
                Status: inquiry.Status,
                InternalNotes: inquiry.InternalNotes,
                CreatedAt: inquiry.CreatedAt,
                UpdatedAt: inquiry.UpdatedAt
            );
        }
    }
}
