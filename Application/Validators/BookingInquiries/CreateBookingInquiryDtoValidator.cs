using Application.DTOs.BookingInquiries;
using FluentValidation;

namespace Application.Validators.BookingInquiries
{
    public class CreateBookingInquiryDtoValidator : AbstractValidator<CreateBookingInquiryDto>
    {
        public CreateBookingInquiryDtoValidator()
        {
            RuleFor(x => x.TripId)
                .NotEmpty().WithMessage("TripId is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(150).WithMessage("Name cannot exceed 150 characters.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.")
                .MaximumLength(150).WithMessage("Email cannot exceed 150 characters.");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("Phone number is required.")
                .MaximumLength(50).WithMessage("Phone number cannot exceed 50 characters.");

            RuleFor(x => x.WhatsApp)
                .MaximumLength(50).When(x => !string.IsNullOrEmpty(x.WhatsApp))
                .WithMessage("WhatsApp number cannot exceed 50 characters.");

            RuleFor(x => x.Nationality)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Nationality))
                .WithMessage("Nationality cannot exceed 100 characters.");

            RuleFor(x => x.HotelOrPickup)
                .MaximumLength(300).When(x => !string.IsNullOrEmpty(x.HotelOrPickup))
                .WithMessage("Hotel/Pickup details cannot exceed 300 characters.");

            RuleFor(x => x.SelectedDate)
                .NotEmpty().WithMessage("Selected date is required.")
                .GreaterThan(DateTime.UtcNow.Date).WithMessage("Selected date cannot be in the past.");

            RuleFor(x => x.Adults)
                .GreaterThanOrEqualTo(1).WithMessage("Number of adults must be at least 1.");

            RuleFor(x => x.Children)
                .GreaterThanOrEqualTo(0).WithMessage("Number of children cannot be negative.");

            RuleFor(x => x.Notes)
                .MaximumLength(2000).When(x => !string.IsNullOrEmpty(x.Notes))
                .WithMessage("Notes cannot exceed 2000 characters.");
        }
    }
}
