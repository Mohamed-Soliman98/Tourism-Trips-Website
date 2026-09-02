using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Trips;
using Domain.Entity;
using Domain.Enum;
using System.Text.RegularExpressions;

namespace Application.Services.Trips
{
    public class GetTripBySlugService : IGetTripBySlugService
    {
        private readonly IUnitOfWork _unitOfWork;
        private static readonly Regex SlugRegex = new(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        public GetTripBySlugService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TripDetailsResponseDto> GetTripBySlugAsync(string slug, Language language, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrWhiteSpace(slug))
            {
                throw new ArgumentException("Trip slug is required.", nameof(slug));
            }

            var trimmedSlug = slug.Trim();
            if (!SlugRegex.IsMatch(trimmedSlug))
            {
                throw new ArgumentException("Invalid slug format. Slug must contain lowercase letters, numbers, and hyphens.", nameof(slug));
            }

            var trip = await _unitOfWork.Trips.GetBySlugWithDetailsAsync(trimmedSlug, cancellationToken);
            if (trip == null || trip.Status != TripStatus.Active)
            {
                throw new KeyNotFoundException($"Active trip with slug '{trimmedSlug}' was not found.");
            }

            return MapToDto(trip, language);
        }

        private static TripDetailsResponseDto MapToDto(Trip trip, Language language)
        {
            var coverImage = trip.Images?.FirstOrDefault(i => i.IsCover);

            // Get the translation for the requested language
            var translation = trip.Translations?.FirstOrDefault(t => t.Language == language);
            
            if (translation == null)
            {
                throw new KeyNotFoundException($"Translation for language '{language}' was not found for this trip.");
            }

            return new TripDetailsResponseDto(
                Id: trip.Id,
                Title: trip.Title,
                Slug: trip.Slug,
                Status: trip.Status,
                IsFeatured: trip.IsFeatured,
                DisplayOrder: trip.DisplayOrder,
                Duration: trip.Duration,
                DurationUnit: trip.DurationUnit,
                PickupLocation: trip.PickupLocation,
                Currency: trip.Currency,
                AdultPrice: trip.AdultPrice,
                ChildPrice: trip.ChildPrice,
                OldPrice: trip.OldPrice,
                IsPriceFrom: trip.IsPriceFrom,
                ShortDescription: trip.ShortDescription,
                LongDescription: trip.LongDescription,
                MetaTitle: trip.MetaTitle,
                MetaDescription: trip.MetaDescription,
                OgImage: trip.OgImage,
                CoverImage: coverImage?.ImageUrl,
                CoverImageAltText: coverImage?.AltText,
                CreatedAt: trip.CreatedAt,
                UpdatedAt: trip.UpdatedAt,
                Category: new CategoryDto(
                    trip.Category?.Id ?? Guid.Empty,
                    trip.Category?.Name ?? string.Empty
                ),
                Destination: new DestinationDto(
                    trip.Destination?.Id ?? Guid.Empty,
                    trip.Destination?.Name ?? string.Empty
                ),
                TourType: new TourTypeDto(
                    trip.TourType?.Id ?? Guid.Empty,
                    trip.TourType?.Name ?? string.Empty
                ),
                Images: trip.Images?
                    .OrderBy(i => i.DisplayOrder)
                    .Select(i => new TripImageDto(
                        i.Id,
                        i.ImageUrl,
                        i.AltText,
                        i.DisplayOrder,
                        i.IsCover))
                    .ToList() ?? new List<TripImageDto>(),
                ItineraryItems: trip.ItineraryItems?
                    .OrderBy(i => i.DisplayOrder)
                    .Select(i => new TripItineraryItemDto(
                        i.Id,
                        i.DisplayOrder,
                        i.Title,
                        i.Description))
                    .ToList() ?? new List<TripItineraryItemDto>(),
                Includes: trip.Includes?
                    .Select(i => new TripIncludeDto(
                        i.Id,
                        i.Description))
                    .ToList() ?? new List<TripIncludeDto>(),
                Excludes: trip.Excludes?
                    .Select(e => new TripExcludeDto(
                        e.Id,
                        e.Description))
                    .ToList() ?? new List<TripExcludeDto>(),
                FAQs: trip.FAQs?
                    .OrderBy(f => f.DisplayOrder)
                    .Select(f => new TripFAQDto(
                        f.Id,
                        f.Question,
                        f.Answer,
                        f.DisplayOrder,
                        f.IsActive,
                        f.Translations?
                            .Select(ft => new FAQTranslationDto(
                                ft.Id,
                                ft.Language,
                                ft.Question,
                                ft.Answer))
                            .ToList() ?? new List<FAQTranslationDto>()))
                    .ToList() ?? new List<TripFAQDto>(),
                Translation: new TripTranslationDto(
                    translation.Id,
                    translation.Language,
                    translation.Title,
                    translation.ShortDescription,
                    translation.LongDescription,
                    translation.MetaTitle,
                    translation.MetaDescription)
            );
        }
    }
}
