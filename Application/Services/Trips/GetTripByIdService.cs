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

        public async Task<PublicTripDetailsResponseDto> GetTripByIdAsync(Guid id, Language? language, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Trip Id cannot be empty.", nameof(id));
            }

            var trip = await _tripRepository.GetByIdWithDetailsAsync(id, cancellationToken);
            if (trip == null || trip.Status != TripStatus.Active)
            {
                throw new KeyNotFoundException($"Active trip with ID '{id}' was not found.");
            }

            return MapToPublicDto(trip, language);
        }

        private static PublicTripDetailsResponseDto MapToPublicDto(Trip trip, Language? language)
        {
            var coverImage = trip.Images?.FirstOrDefault(i => i.IsCover);
            var requestedLanguage = language ?? Language.English;

            var translation =
                trip.Translations?.FirstOrDefault(t => t.Language == requestedLanguage)
                ?? trip.Translations?.FirstOrDefault(t => t.Language == Language.English);

            if (translation == null)
            {
                throw new InvalidOperationException($"Data integrity error: Required translation for trip '{trip.Id}' was not found.");
            }

            return new PublicTripDetailsResponseDto(
                Id: trip.Id,
                Title: translation.Title,
                Slug: trip.Slug,
                Status: trip.Status,
                IsFeatured: trip.IsFeatured,
                DisplayOrder: trip.DisplayOrder,
                Duration: trip.Duration,
                DurationUnit: trip.DurationUnit,
                PickupLocation: translation.PickupLocation ?? trip.PickupLocation,
                Currency: trip.Currency,
                AdultPrice: trip.AdultPrice,
                ChildPrice: trip.ChildPrice,
                OldPrice: trip.OldPrice,
                IsPriceFrom: trip.IsPriceFrom,
                ShortDescription: translation.ShortDescription,
                LongDescription: translation.LongDescription,
                MetaTitle: translation.MetaTitle ?? trip.MetaTitle,
                MetaDescription: translation.MetaDescription ?? trip.MetaDescription,
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
                    .Select(i =>
                    {
                        var itemTr = i.Translations?.FirstOrDefault(t => t.Language == requestedLanguage)
                                    ?? i.Translations?.FirstOrDefault(t => t.Language == Language.English);
                        return new TripItineraryItemDto(
                            i.Id,
                            i.DisplayOrder,
                            itemTr?.Title ?? string.Empty,
                            itemTr?.Description);
                    })
                    .ToList() ?? new List<TripItineraryItemDto>(),
                Includes: trip.Includes?
                    .Select(i =>
                    {
                        var incTr = i.Translations?.FirstOrDefault(t => t.Language == requestedLanguage)
                                   ?? i.Translations?.FirstOrDefault(t => t.Language == Language.English);
                        return new TripIncludeDto(
                            i.Id,
                            incTr?.Description ?? string.Empty);
                    })
                    .ToList() ?? new List<TripIncludeDto>(),
                Excludes: trip.Excludes?
                    .Select(e =>
                    {
                        var excTr = e.Translations?.FirstOrDefault(t => t.Language == requestedLanguage)
                               ?? e.Translations?.FirstOrDefault(t => t.Language == Language.English);
                        return new TripExcludeDto(
                            e.Id,
                            excTr?.Description ?? string.Empty);
                    })
                    .ToList() ?? new List<TripExcludeDto>(),
                Highlights: trip.Highlights?
                    .OrderBy(h => h.DisplayOrder)
                    .Select(h =>
                    {
                        var hlTr = h.Translations?.FirstOrDefault(t => t.Language == requestedLanguage)
                                  ?? h.Translations?.FirstOrDefault(t => t.Language == Language.English);
                        return new TripHighlightDto(
                            h.Id,
                            hlTr?.Description ?? string.Empty,
                            h.DisplayOrder);
                    })
                    .ToList() ?? new List<TripHighlightDto>(),
                WhatToBringItems: trip.WhatToBringItems?
                    .OrderBy(w => w.DisplayOrder)
                    .Select(w =>
                    {
                        var wtbTr = w.Translations?.FirstOrDefault(t => t.Language == requestedLanguage)
                                   ?? w.Translations?.FirstOrDefault(t => t.Language == Language.English);
                        return new TripWhatToBringDto(
                            w.Id,
                            wtbTr?.Description ?? string.Empty,
                            w.DisplayOrder);
                    })
                    .ToList() ?? new List<TripWhatToBringDto>(),
                FAQs: trip.FAQs?
                    .Where(f => f.IsActive)
                    .OrderBy(f => f.DisplayOrder)
                    .Select(f =>
                    {
                        var faqTr = f.Translations?.FirstOrDefault(t => t.Language == requestedLanguage)
                                   ?? f.Translations?.FirstOrDefault(t => t.Language == Language.English);
                        return new PublicTripFAQDto(
                            f.Id,
                            faqTr?.Question ?? f.Question,
                            faqTr?.Answer ?? f.Answer,
                            f.DisplayOrder,
                            f.IsActive);
                    })
                    .ToList() ?? new List<PublicTripFAQDto>()
            );
        }
    }
}