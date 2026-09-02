using Application.DTOs.Destinations;
using FluentValidation;

namespace Application.Validators.Destinations
{
    public class CreateDestinationDtoValidator : AbstractValidator<CreateDestinationDto>
    {
        public CreateDestinationDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Destination name is required.")
                .MaximumLength(100).WithMessage("Destination name cannot exceed 100 characters.");
        }
    }
}