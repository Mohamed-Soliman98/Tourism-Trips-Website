namespace Application.DTOs.BookingInquiries
{
    public sealed record CreateBookingInquiryDto(
        Guid TripId,
        string Name,
        string Email,
        string Phone,
        string? WhatsApp,
        string? Nationality,
        string? HotelOrPickup,
        DateTime SelectedDate,
        int Adults = 1,
        int Children = 0,
        string? Notes = null
    );
}
