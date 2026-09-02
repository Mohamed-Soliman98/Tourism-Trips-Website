using Application.DTOs.TripImages;
using FluentValidation;

namespace Application.Validators.TripImages
{
    public class UpdateTripImageDtoValidator : AbstractValidator<UpdateTripImageDto>
    {
        public UpdateTripImageDtoValidator()
        {
            RuleFor(x => x.AltText)
                .MaximumLength(200).WithMessage("Alt text cannot exceed 200 characters.")
                .When(x => x.AltText != null);

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Display order must be 0 or greater.");
        }
    }
}
