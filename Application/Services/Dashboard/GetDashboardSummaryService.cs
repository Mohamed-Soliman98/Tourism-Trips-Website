using Application.DTOs.Dashboard;
using Application.Interfaces.Dashboard;
using Application.Interfaces.IUnitOfWork;
using Domain.Enum;

namespace Application.Services.Dashboard
{
    public class GetDashboardSummaryService : IGetDashboardSummaryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDashboardSummaryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default)
        {
            var totalTrips = await _unitOfWork.Trips.GetTotalCountAsync(cancellationToken);
            var activeTrips = await _unitOfWork.Trips.GetCountByStatusAsync(TripStatus.Active, cancellationToken);
            var draftTrips = await _unitOfWork.Trips.GetCountByStatusAsync(TripStatus.Draft, cancellationToken);

            var totalBookingInquiries = await _unitOfWork.BookingInquiries.GetTotalCountAsync(cancellationToken);
            var newBookingInquiries = await _unitOfWork.BookingInquiries.GetCountByStatusAsync(BookingInquiryStatus.New, cancellationToken);
            var contactedBookingInquiries = await _unitOfWork.BookingInquiries.GetCountByStatusAsync(BookingInquiryStatus.Contacted, cancellationToken);
            var confirmedBookingInquiries = await _unitOfWork.BookingInquiries.GetCountByStatusAsync(BookingInquiryStatus.Confirmed, cancellationToken);
            var cancelledBookingInquiries = await _unitOfWork.BookingInquiries.GetCountByStatusAsync(BookingInquiryStatus.Cancelled, cancellationToken);

            var totalTestimonials = await _unitOfWork.Testimonials.GetTotalCountAsync(cancellationToken);
            var activeTestimonials = await _unitOfWork.Testimonials.GetActiveCountAsync(cancellationToken);
            var inactiveTestimonials = await _unitOfWork.Testimonials.GetInactiveCountAsync(cancellationToken);

            var totalBanners = await _unitOfWork.Banners.GetTotalCountAsync(cancellationToken);
            var activeBanners = await _unitOfWork.Banners.GetActiveCountAsync(cancellationToken);
            var inactiveBanners = await _unitOfWork.Banners.GetInactiveCountAsync(cancellationToken);

            return new DashboardSummaryDto(
                TotalTrips: totalTrips,
                ActiveTrips: activeTrips,
                DraftTrips: draftTrips,
                TotalBookingInquiries: totalBookingInquiries,
                NewBookingInquiries: newBookingInquiries,
                ContactedBookingInquiries: contactedBookingInquiries,
                ConfirmedBookingInquiries: confirmedBookingInquiries,
                CancelledBookingInquiries: cancelledBookingInquiries,
                TotalTestimonials: totalTestimonials,
                ActiveTestimonials: activeTestimonials,
                InactiveTestimonials: inactiveTestimonials,
                TotalBanners: totalBanners,
                ActiveBanners: activeBanners,
                InactiveBanners: inactiveBanners
            );
        }
    }
}
