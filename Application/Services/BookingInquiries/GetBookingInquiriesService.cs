using Application.DTOs.BookingInquiries;
using Application.DTOs.Common;
using Application.Interfaces.BookingInquiries;
using Application.Interfaces.Repositories;
using Domain.Entity;
using FluentValidation;

namespace Application.Services.BookingInquiries
{
    public class GetBookingInquiriesService : IGetBookingInquiriesService
    {
        private readonly IBookingInquiryRepository _bookingInquiryRepository;
        private readonly IValidator<GetBookingInquiriesQueryDto> _validator;

        public GetBookingInquiriesService(
            IBookingInquiryRepository bookingInquiryRepository,
            IValidator<GetBookingInquiriesQueryDto> validator)
        {
            _bookingInquiryRepository = bookingInquiryRepository;
            _validator = validator;
        }

        public async Task<PagedResult<BookingInquirySummaryDto>> GetBookingInquiriesAsync(
            GetBookingInquiriesQueryDto query,
            CancellationToken cancellationToken = default)
        {
            await _validator.ValidateAndThrowAsync(query, cancellationToken);

            var (items, totalCount) = await _bookingInquiryRepository.GetInquiriesAsync(query, cancellationToken);

            var dtos = items.Select(MapToSummaryDto).ToList();

            return new PagedResult<BookingInquirySummaryDto>(
                dtos,
                query.Page,
                query.PageSize,
                totalCount
            );
        }

        private static BookingInquirySummaryDto MapToSummaryDto(BookingInquiry inquiry)
        {
            return new BookingInquirySummaryDto(
                Id: inquiry.Id,
                TripId: inquiry.TripId,
                TripTitle: inquiry.Trip?.Title ?? string.Empty,
                Name: inquiry.Name,
                Email: inquiry.Email,
                Phone: inquiry.Phone,
                WhatsApp: inquiry.WhatsApp,
                SelectedDate: inquiry.SelectedDate,
                Adults: inquiry.Adults,
                Children: inquiry.Children,
                Status: inquiry.Status,
                CreatedAt: inquiry.CreatedAt
            );
        }
    }
}
