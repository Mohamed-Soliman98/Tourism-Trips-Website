using Application.DTOs.Trips;
using FluentValidation;

namespace Application.Validators.Trips
{
    public class CreateTripDtoValidator : AbstractValidator<CreateTripDto>
    {
        public CreateTripDtoValidator()
        {
            RuleFor(x => x.Title)
                .NotNull().WithMessage("Title is required.");

            When(x => x.Title != null, () =>
            {
                RuleFor(x => x.Title.English)
                    .NotEmpty().WithMessage("English title is required.")
                    .MaximumLength(200).WithMessage("English title cannot exceed 200 characters.");

                RuleFor(x => x.Title.German)
                    .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Title.German))
                    .WithMessage("German title cannot exceed 200 characters.");
            });

            RuleFor(x => x.Slug)
                .NotEmpty().WithMessage("Slug is required.")
                .MaximumLength(200).WithMessage("Slug cannot exceed 200 characters.")
                .Matches(@"^[a-z0-9]+(?:-[a-z0-9]+)*$")
                .WithMessage("Slug must be lowercase alphanumeric characters separated by hyphens (e.g. 'cairo-day-tour').");

            RuleFor(x => x.Duration)
                .GreaterThan(0).WithMessage("Duration must be greater than 0.");

            RuleFor(x => x.DurationUnit)
                .IsInEnum().WithMessage("Invalid duration unit.");

            RuleFor(x => x.Currency)
                .NotEmpty().WithMessage("Currency is required.")
                .MaximumLength(10).WithMessage("Currency cannot exceed 10 characters.");

            RuleFor(x => x.AdultPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Adult price cannot be negative.");

            RuleFor(x => x.ChildPrice)
                .GreaterThanOrEqualTo(0).WithMessage("Child price cannot be negative.");

            RuleFor(x => x.OldPrice)
                .GreaterThanOrEqualTo(0).When(x => x.OldPrice.HasValue)
                .WithMessage("Old price cannot be negative.");

            RuleFor(x => x.ShortDescription)
                .NotNull().WithMessage("Short description is required.");

            When(x => x.ShortDescription != null, () =>
            {
                RuleFor(x => x.ShortDescription.English)
                    .NotEmpty().WithMessage("English short description is required.")
                    .MaximumLength(500).WithMessage("English short description cannot exceed 500 characters.");

                RuleFor(x => x.ShortDescription.German)
                    .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.ShortDescription.German))
                    .WithMessage("German short description cannot exceed 500 characters.");
            });

            RuleFor(x => x.LongDescription)
                .NotNull().WithMessage("Long description is required.");

            When(x => x.LongDescription != null, () =>
            {
                RuleFor(x => x.LongDescription.English)
                    .NotEmpty().WithMessage("English long description is required.")
                    .MaximumLength(5000).WithMessage("English long description cannot exceed 5000 characters.");

                RuleFor(x => x.LongDescription.German)
                    .MaximumLength(5000).When(x => !string.IsNullOrEmpty(x.LongDescription.German))
                    .WithMessage("German long description cannot exceed 5000 characters.");
            });

            When(x => x.MetaTitle != null, () =>
            {
                RuleFor(x => x.MetaTitle!.English)
                    .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.MetaTitle?.English))
                    .WithMessage("English meta title cannot exceed 200 characters.");

                RuleFor(x => x.MetaTitle!.German)
                    .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.MetaTitle?.German))
                    .WithMessage("German meta title cannot exceed 200 characters.");
            });

            When(x => x.MetaDescription != null, () =>
            {
                RuleFor(x => x.MetaDescription!.English)
                    .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.MetaDescription?.English))
                    .WithMessage("English meta description cannot exceed 500 characters.");

                RuleFor(x => x.MetaDescription!.German)
                    .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.MetaDescription?.German))
                    .WithMessage("German meta description cannot exceed 500 characters.");
            });

            When(x => x.PickupLocation != null, () =>
            {
                RuleFor(x => x.PickupLocation!.English)
                    .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.PickupLocation?.English))
                    .WithMessage("English pickup location cannot exceed 500 characters.");

                RuleFor(x => x.PickupLocation!.German)
                    .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.PickupLocation?.German))
                    .WithMessage("German pickup location cannot exceed 500 characters.");
            });

            RuleFor(x => x.Notes)
                .MaximumLength(5000).WithMessage("Notes cannot exceed 5000 characters.");

            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category ID is required.");

            RuleFor(x => x.DestinationId)
                .NotEmpty().WithMessage("Destination ID is required.");

            RuleFor(x => x.TourTypeId)
                .NotEmpty().WithMessage("Tour Type ID is required.");

            RuleForEach(x => x.ItineraryItems)
                .SetValidator(new CreateTripItineraryItemDtoValidator());

            RuleForEach(x => x.Includes)
                .SetValidator(new CreateTripIncludeDtoValidator());

            RuleForEach(x => x.Excludes)
                .SetValidator(new CreateTripExcludeDtoValidator());

            RuleForEach(x => x.Highlights)
                .SetValidator(new CreateTripHighlightDtoValidator());

            RuleForEach(x => x.WhatToBringItems)
                .SetValidator(new CreateTripWhatToBringDtoValidator());

            RuleForEach(x => x.FAQs)
                .SetValidator(new CreateTripFAQDtoValidator());
        }
    }
}
