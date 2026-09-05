using Application.DTOs.BookingInquiries;
using Application.Interfaces.BookingInquiries;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.Services.BookingInquiries
{
    public class UpdateBookingInquiryService : IUpdateBookingInquiryService
    {
        private readonly IBookingInquiryRepository _bookingInquiryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateBookingInquiryDto> _validator;

        public UpdateBookingInquiryService(
            IUnitOfWork unitOfWork,
            IBookingInquiryRepository bookingInquiryRepository,
            IValidator<UpdateBookingInquiryDto> validator)
        {
            _unitOfWork = unitOfWork;
            _bookingInquiryRepository = bookingInquiryRepository;
            _validator = validator;
        }

        public async Task<BookingInquiryUpdatedResponseDto> UpdateBookingInquiryAsync(
            Guid id,
            UpdateBookingInquiryDto dto,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Booking inquiry Id cannot be empty.", nameof(id));
            }

            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            var inquiry = await _bookingInquiryRepository.GetByIdAsync(id, cancellationToken);

            if (inquiry == null)
            {
                throw new KeyNotFoundException($"Booking inquiry with ID '{id}' was not found.");
            }

            inquiry.Status = dto.Status;
            inquiry.InternalNotes = string.IsNullOrWhiteSpace(dto.InternalNotes) ? null : dto.InternalNotes.Trim();
            inquiry.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new BookingInquiryUpdatedResponseDto(
                Id: inquiry.Id,
                Status: inquiry.Status,
                Message: "Booking inquiry updated successfully."
            );
        }
    }
}
