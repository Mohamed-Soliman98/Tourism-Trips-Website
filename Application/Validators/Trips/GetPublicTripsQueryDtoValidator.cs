using Application.DTOs.Trips;
using FluentValidation;

namespace Application.Validators.Trips
{
    public class GetPublicTripsQueryDtoValidator : AbstractValidator<GetPublicTripsQueryDto>
    {
        public GetPublicTripsQueryDtoValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("Page size must be at least 1.")
                .LessThanOrEqualTo(50).WithMessage("Page size cannot exceed 50 items per page.");

            RuleFor(x => x.Search)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Search))
                .WithMessage("Search query cannot exceed 100 characters.");

            RuleFor(x => x.CategoryId)
                .Must(id => id == null || id != Guid.Empty)
                .WithMessage("CategoryId must be a valid non-empty GUID when supplied.");

            RuleFor(x => x.DestinationId)
                .Must(id => id == null || id != Guid.Empty)
                .WithMessage("DestinationId must be a valid non-empty GUID when supplied.");

            RuleFor(x => x.TourTypeId)
                .Must(id => id == null || id != Guid.Empty)
                .WithMessage("TourTypeId must be a valid non-empty GUID when supplied.");

            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0).When(x => x.MinPrice.HasValue)
                .WithMessage("MinPrice cannot be negative.");

            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(0).When(x => x.MaxPrice.HasValue)
                .WithMessage("MaxPrice cannot be negative.");

            RuleFor(x => x)
                .Must(x => !x.MinPrice.HasValue || !x.MaxPrice.HasValue || x.MinPrice.Value <= x.MaxPrice.Value)
                .WithMessage("MinPrice cannot be greater than MaxPrice.");

            RuleFor(x => x.Language)
                .IsInEnum().When(x => x.Language.HasValue)
                .WithMessage("Invalid language value.");
        }
    }
}
