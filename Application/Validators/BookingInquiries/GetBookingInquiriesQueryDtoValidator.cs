using Application.DTOs.BookingInquiries;
using FluentValidation;

namespace Application.Validators.BookingInquiries
{
    public class GetBookingInquiriesQueryDtoValidator : AbstractValidator<GetBookingInquiriesQueryDto>
    {
        public GetBookingInquiriesQueryDtoValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("Page size must be at least 1.")
                .LessThanOrEqualTo(50).WithMessage("Page size cannot exceed 50 items per page.");

            RuleFor(x => x.Search)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Search))
                .WithMessage("Search query cannot exceed 100 characters.");

            RuleFor(x => x.FromDate)
                .LessThanOrEqualTo(x => x.ToDate!.Value)
                .When(x => x.FromDate.HasValue && x.ToDate.HasValue)
                .WithMessage("FromDate must be before or equal to ToDate.");
        }
    }
}
