using Application.DTOs.Dashboard;
using Application.Interfaces.Dashboard;
using Application.Interfaces.Repositories;
using Domain.Enum;

namespace Application.Services.Dashboard
{
    public class GetDashboardSummaryService : IGetDashboardSummaryService
    {
        private readonly ITripRepository _tripRepository;
        private readonly IBookingInquiryRepository _bookingInquiryRepository;
        private readonly ITestimonialRepository _testimonialRepository;
        private readonly IBannerRepository _bannerRepository;

        public GetDashboardSummaryService(
            ITripRepository tripRepository,
            IBookingInquiryRepository bookingInquiryRepository,
            ITestimonialRepository testimonialRepository,
            IBannerRepository bannerRepository)
        {
            _tripRepository = tripRepository;
            _bookingInquiryRepository = bookingInquiryRepository;
            _testimonialRepository = testimonialRepository;
            _bannerRepository = bannerRepository;
        }

        public async Task<DashboardSummaryDto> GetDashboardSummaryAsync(CancellationToken cancellationToken = default)
        {
            var totalTrips = await _tripRepository.GetTotalCountAsync(cancellationToken);
            var activeTrips = await _tripRepository.GetCountByStatusAsync(TripStatus.Active, cancellationToken);
            var draftTrips = await _tripRepository.GetCountByStatusAsync(TripStatus.Draft, cancellationToken);

            var totalBookingInquiries = await _bookingInquiryRepository.GetTotalCountAsync(cancellationToken);
            var newBookingInquiries = await _bookingInquiryRepository.GetCountByStatusAsync(BookingInquiryStatus.New, cancellationToken);
            var contactedBookingInquiries = await _bookingInquiryRepository.GetCountByStatusAsync(BookingInquiryStatus.Contacted, cancellationToken);
            var confirmedBookingInquiries = await _bookingInquiryRepository.GetCountByStatusAsync(BookingInquiryStatus.Confirmed, cancellationToken);
            var cancelledBookingInquiries = await _bookingInquiryRepository.GetCountByStatusAsync(BookingInquiryStatus.Cancelled, cancellationToken);

            var totalTestimonials = await _testimonialRepository.GetTotalCountAsync(cancellationToken);
            var activeTestimonials = await _testimonialRepository.GetActiveCountAsync(cancellationToken);
            var inactiveTestimonials = await _testimonialRepository.GetInactiveCountAsync(cancellationToken);

            var totalBanners = await _bannerRepository.GetTotalCountAsync(cancellationToken);
            var activeBanners = await _bannerRepository.GetActiveCountAsync(cancellationToken);
            var inactiveBanners = await _bannerRepository.GetInactiveCountAsync(cancellationToken);

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
