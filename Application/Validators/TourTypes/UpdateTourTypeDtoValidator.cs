using Application.DTOs.TourTypes;
using FluentValidation;

namespace Application.Validators.TourTypes
{
    public class UpdateTourTypeDtoValidator : AbstractValidator<UpdateTourTypeDto>
    {
        public UpdateTourTypeDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Tour type name is required.")
                .MaximumLength(100).WithMessage("Tour type name cannot exceed 100 characters.");
        }
    }
}
