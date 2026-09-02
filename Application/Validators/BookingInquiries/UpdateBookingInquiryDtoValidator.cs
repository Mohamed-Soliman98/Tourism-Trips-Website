using Application.DTOs.BookingInquiries;
using FluentValidation;

namespace Application.Validators.BookingInquiries
{
    public class UpdateBookingInquiryDtoValidator : AbstractValidator<UpdateBookingInquiryDto>
    {
        public UpdateBookingInquiryDtoValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid booking inquiry status.");

            RuleFor(x => x.InternalNotes)
                .MaximumLength(2000).When(x => !string.IsNullOrEmpty(x.InternalNotes))
                .WithMessage("Internal notes cannot exceed 2000 characters.");
        }
    }
}
