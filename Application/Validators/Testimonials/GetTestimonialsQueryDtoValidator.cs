using Application.DTOs.Testimonials;
using FluentValidation;

namespace Application.Validators.Testimonials
{
    public class GetTestimonialsQueryDtoValidator : AbstractValidator<GetTestimonialsQueryDto>
    {
        public GetTestimonialsQueryDtoValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThan(0).WithMessage("Page must be greater than 0.");

            RuleFor(x => x.PageSize)
                .GreaterThan(0).WithMessage("Page size must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Page size cannot exceed 100.");

            RuleFor(x => x.SearchTerm)
                .MaximumLength(200).WithMessage("Search term cannot exceed 200 characters.")
                .When(x => !string.IsNullOrEmpty(x.SearchTerm));
        }
    }
}