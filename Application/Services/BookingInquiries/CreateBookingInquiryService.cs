using Application.DTOs.BookingInquiries;
using Application.Interfaces.BookingInquiries;
using Application.Interfaces.IUnitOfWork;
using Domain.Entity;
using Domain.Enum;
using FluentValidation;

namespace Application.Services.BookingInquiries
{
    public class CreateBookingInquiryService : ICreateBookingInquiryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateBookingInquiryDto> _validator;

        public CreateBookingInquiryService(
            IUnitOfWork unitOfWork,
            IValidator<CreateBookingInquiryDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<BookingInquiryCreatedResponseDto> CreateBookingInquiryAsync(
            CreateBookingInquiryDto dto,
            CancellationToken cancellationToken = default)
        {
            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            if (dto.SelectedDate.Date <= DateTime.UtcNow.Date)
            {
                throw new ArgumentException("Selected date must be a future date.");
            }
            var trip = await _unitOfWork.Trips.GetByIdWithDetailsAsync(dto.TripId, cancellationToken);
            if (trip == null || trip.Status != TripStatus.Active)
            {
                throw new KeyNotFoundException($"Active trip with ID '{dto.TripId}' was not found.");
            }

            var inquiry = new BookingInquiry
            {
                Id = Guid.NewGuid(),
                TripId = dto.TripId,
                Name = dto.Name.Trim(),
                Email = dto.Email.Trim().ToLowerInvariant(),
                Phone = dto.Phone.Trim(),
                WhatsApp = string.IsNullOrWhiteSpace(dto.WhatsApp) ? null : dto.WhatsApp.Trim(),
                Nationality = string.IsNullOrWhiteSpace(dto.Nationality) ? null : dto.Nationality.Trim(),
                HotelOrPickup = string.IsNullOrWhiteSpace(dto.HotelOrPickup) ? null : dto.HotelOrPickup.Trim(),
                SelectedDate = dto.SelectedDate,
                Adults = dto.Adults,
                Children = dto.Children,
                Notes = string.IsNullOrWhiteSpace(dto.Notes) ? null : dto.Notes.Trim(),
                Status = BookingInquiryStatus.New,
                CreatedAt = DateTime.UtcNow
            };

            _unitOfWork.BookingInquiries.Add(inquiry);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new BookingInquiryCreatedResponseDto(
                inquiry.Id,
                inquiry.TripId,
                "Booking inquiry submitted successfully."
            );
        }
    }
}
