namespace Application.DTOs.BookingInquiries
{
    public sealed record BookingInquiryCreatedResponseDto(
        Guid Id,
        Guid TripId,
        string Message = "Booking inquiry submitted successfully."
    );
}
