namespace Application.DTOs.Dashboard
{
    public sealed record DashboardSummaryDto(
        int TotalTrips,
        int ActiveTrips,
        int DraftTrips,
        int TotalBookingInquiries,
        int NewBookingInquiries,
        int ContactedBookingInquiries,
        int ConfirmedBookingInquiries,
        int CancelledBookingInquiries,
        int TotalTestimonials,
        int ActiveTestimonials,
        int InactiveTestimonials,
        int TotalBanners,
        int ActiveBanners,
        int InactiveBanners
    );
}
