namespace Application.DTOs.BookingInquiries
{
    public sealed record BookingInquiryDeletedResponseDto(
        string Message = "Booking inquiry deleted successfully."
    );
}
