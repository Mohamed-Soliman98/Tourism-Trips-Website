using Application.DTOs.Trips;
using Domain.Enum;
using FluentValidation;

namespace Application.Validators.Trips
{
    public class UpdateTripItineraryItemDtoValidator : AbstractValidator<UpdateTripItineraryItemDto>
    {
        public UpdateTripItineraryItemDtoValidator()
        {
            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Display order must be greater than or equal to 0.");

            RuleFor(x => x.Title)
                .NotNull().WithMessage("Itinerary item title is required.");

            When(x => x.Title != null, () =>
            {
                RuleFor(x => x.Title.English)
                    .NotEmpty().WithMessage("Itinerary item English title is required.")
                    .MaximumLength(200).WithMessage("Itinerary item English title cannot exceed 200 characters.");

                RuleFor(x => x.Title.German)
                    .MaximumLength(200).When(x => !string.IsNullOrEmpty(x.Title.German))
                    .WithMessage("Itinerary item German title cannot exceed 200 characters.");
            });

            When(x => x.Description != null, () =>
            {
                RuleFor(x => x.Description!.English)
                    .MaximumLength(1000).When(x => !string.IsNullOrEmpty(x.Description?.English))
                    .WithMessage("Itinerary item English description cannot exceed 1000 characters.");

                RuleFor(x => x.Description!.German)
                    .MaximumLength(1000).When(x => !string.IsNullOrEmpty(x.Description?.German))
                    .WithMessage("Itinerary item German description cannot exceed 1000 characters.");
            });
        }
    }

    public class UpdateTripIncludeDtoValidator : AbstractValidator<UpdateTripIncludeDto>
    {
        public UpdateTripIncludeDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotNull().WithMessage("Include description is required.");

            RuleFor(x => x.Description.English)
                .NotEmpty().WithMessage("Include English description is required.")
                .MaximumLength(500).WithMessage("Include English description cannot exceed 500 characters.");

            RuleFor(x => x.Description.German)
                .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Description?.German))
                .WithMessage("Include German description cannot exceed 500 characters.");
        }
    }

    public class UpdateTripExcludeDtoValidator : AbstractValidator<UpdateTripExcludeDto>
    {
        public UpdateTripExcludeDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotNull().WithMessage("Exclude description is required.");

            RuleFor(x => x.Description.English)
                .NotEmpty().WithMessage("Exclude English description is required.")
                .MaximumLength(500).WithMessage("Exclude English description cannot exceed 500 characters.");

            RuleFor(x => x.Description.German)
                .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Description?.German))
                .WithMessage("Exclude German description cannot exceed 500 characters.");
        }
    }

    public class UpdateTripHighlightDtoValidator : AbstractValidator<UpdateTripHighlightDto>
    {
        public UpdateTripHighlightDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotNull().WithMessage("Highlight description is required.");

            RuleFor(x => x.Description.English)
                .NotEmpty().WithMessage("Highlight English description is required.")
                .MaximumLength(500).WithMessage("Highlight English description cannot exceed 500 characters.");

            RuleFor(x => x.Description.German)
                .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Description?.German))
                .WithMessage("Highlight German description cannot exceed 500 characters.");

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Display order must be greater than or equal to 0.");
        }
    }

    public class UpdateTripWhatToBringDtoValidator : AbstractValidator<UpdateTripWhatToBringDto>
    {
        public UpdateTripWhatToBringDtoValidator()
        {
            RuleFor(x => x.Description)
                .NotNull().WithMessage("What to bring description is required.");

            RuleFor(x => x.Description.English)
                .NotEmpty().WithMessage("What to bring English description is required.")
                .MaximumLength(500).WithMessage("What to bring English description cannot exceed 500 characters.");

            RuleFor(x => x.Description.German)
                .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Description?.German))
                .WithMessage("What to bring German description cannot exceed 500 characters.");

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Display order must be greater than or equal to 0.");
        }
    }

    public class UpdateTripFAQDtoValidator : AbstractValidator<UpdateTripFAQDto>
    {
        public UpdateTripFAQDtoValidator()
        {
            RuleFor(x => x.Question)
                .NotNull().WithMessage("FAQ question is required.");

            RuleFor(x => x.Question.English)
                .NotEmpty().WithMessage("FAQ English question is required.")
                .MaximumLength(500).WithMessage("FAQ English question cannot exceed 500 characters.");

            RuleFor(x => x.Question.German)
                .MaximumLength(500).When(x => !string.IsNullOrEmpty(x.Question?.German))
                .WithMessage("FAQ German question cannot exceed 500 characters.");

            RuleFor(x => x.Answer)
                .NotNull().WithMessage("FAQ answer is required.");

            RuleFor(x => x.Answer.English)
                .NotEmpty().WithMessage("FAQ English answer is required.")
                .MaximumLength(2000).WithMessage("FAQ English answer cannot exceed 2000 characters.");

            RuleFor(x => x.Answer.German)
                .MaximumLength(2000).When(x => !string.IsNullOrEmpty(x.Answer?.German))
                .WithMessage("FAQ German answer cannot exceed 2000 characters.");

            RuleFor(x => x.DisplayOrder)
                .GreaterThanOrEqualTo(0).WithMessage("Display order must be greater than or equal to 0.");
        }
    }

    public class UpdateTripDtoValidator : AbstractValidator<UpdateTripDto>
    {
        public UpdateTripDtoValidator()
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

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid trip status.");

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
                .SetValidator(new UpdateTripItineraryItemDtoValidator());

            RuleForEach(x => x.Includes)
                .SetValidator(new UpdateTripIncludeDtoValidator());

            RuleForEach(x => x.Excludes)
                .SetValidator(new UpdateTripExcludeDtoValidator());

            RuleForEach(x => x.Highlights)
                .SetValidator(new UpdateTripHighlightDtoValidator());

            RuleForEach(x => x.WhatToBringItems)
                .SetValidator(new UpdateTripWhatToBringDtoValidator());

            RuleForEach(x => x.FAQs)
                .SetValidator(new UpdateTripFAQDtoValidator());
        }
    }
}
