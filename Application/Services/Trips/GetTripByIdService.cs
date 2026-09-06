using Application.DTOs.Trips;
using Application.Interfaces.Repositories;
using Application.Interfaces.Trips;
using Domain.Entity;
using Domain.Enum;

namespace Application.Services.Trips
{
    public class GetTripByIdService : IGetTripByIdService
    {
        private readonly ITripRepository _tripRepository;

        public GetTripByIdService(ITripRepository tripRepository)
        {
            _tripRepository = tripRepository;
        }

        public async Task<TripDetailsResponseDto> GetTripByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Trip Id cannot be empty.", nameof(id));
            }

            var trip = await _tripRepository.GetByIdWithDetailsAsync(id, cancellationToken);
            if (trip == null)
            {
                throw new KeyNotFoundException($"Trip with ID '{id}' was not found.");
            }

            return MapToDto(trip);
        }

        private static TripDetailsResponseDto MapToDto(Trip trip)
        {
            var coverImage = trip.Images?.FirstOrDefault(i => i.IsCover);
            var translation = trip.Translations?.FirstOrDefault(t => t.Language == Language.English)
                           ?? trip.Translations?.FirstOrDefault();

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
                Notes: trip.Notes,
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
                Highlights: trip.Highlights?
                    .OrderBy(h => h.DisplayOrder)
                    .Select(h => new TripHighlightDto(
                        h.Id,
                        h.Description,
                        h.DisplayOrder))
                    .ToList() ?? new List<TripHighlightDto>(),
                WhatToBringItems: trip.WhatToBringItems?
                    .OrderBy(w => w.DisplayOrder)
                    .Select(w => new TripWhatToBringDto(
                        w.Id,
                        w.Description,
                        w.DisplayOrder))
                    .ToList() ?? new List<TripWhatToBringDto>(),
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
                Translation: translation != null ? new TripTranslationDto(
                    translation.Id,
                    translation.Language,
                    translation.Title,
                    translation.ShortDescription,
                    translation.LongDescription,
                    translation.MetaTitle,
                    translation.MetaDescription) : null
            );
        }
    }
}
