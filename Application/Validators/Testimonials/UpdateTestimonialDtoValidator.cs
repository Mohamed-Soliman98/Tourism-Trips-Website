using Application.DTOs.Testimonials;
using FluentValidation;

namespace Application.Validators.Testimonials
{
    public class UpdateTestimonialDtoValidator : AbstractValidator<UpdateTestimonialDto>
    {
        public UpdateTestimonialDtoValidator()
        {
            RuleFor(x => x.CustomerName)
                .NotEmpty().WithMessage("Customer name is required.")
                .MaximumLength(200).WithMessage("Customer name cannot exceed 200 characters.");

            RuleFor(x => x.Country)
                .MaximumLength(100).WithMessage("Country cannot exceed 100 characters.")
                .When(x => !string.IsNullOrEmpty(x.Country));

            RuleFor(x => x.Rating)
                .NotEmpty().WithMessage("Rating is required.")
                .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Content is required.")
                .MaximumLength(2000).WithMessage("Content cannot exceed 2000 characters.");
        }
    }
}