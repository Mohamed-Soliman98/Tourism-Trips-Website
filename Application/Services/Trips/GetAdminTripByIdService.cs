using Application.DTOs.Trips;
using Application.Interfaces.Repositories;
using Application.Interfaces.Trips;
using Domain.Entity;

namespace Application.Services.Trips
{
    public class GetAdminTripByIdService : IGetAdminTripByIdService
    {
        private readonly ITripRepository _tripRepository;

        public GetAdminTripByIdService(ITripRepository tripRepository)
        {
            _tripRepository = tripRepository;
        }

        public async Task<AdminTripDetailsResponseDto> GetAdminTripByIdAsync(Guid id, CancellationToken cancellationToken = default)
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

            return MapToAdminDto(trip);
        }

        private static AdminTripDetailsResponseDto MapToAdminDto(Trip trip)
        {
            var coverImage = trip.Images?.FirstOrDefault(i => i.IsCover);

            // For admin, use English as default fallback values but don't filter
            var englishTranslation = trip.Translations?.FirstOrDefault(t => t.Language == Domain.Enum.Language.English);
            
            return new AdminTripDetailsResponseDto(
                Id: trip.Id,
                Title: englishTranslation?.Title ?? trip.Title,
                Slug: trip.Slug,
                Status: trip.Status,
                IsFeatured: trip.IsFeatured,
                DisplayOrder: trip.DisplayOrder,
                Duration: trip.Duration,
                DurationUnit: trip.DurationUnit,
                PickupLocation: englishTranslation?.PickupLocation ?? trip.PickupLocation,
                Currency: trip.Currency,
                AdultPrice: trip.AdultPrice,
                ChildPrice: trip.ChildPrice,
                OldPrice: trip.OldPrice,
                IsPriceFrom: trip.IsPriceFrom,
                ShortDescription: englishTranslation?.ShortDescription ?? trip.ShortDescription,
                LongDescription: englishTranslation?.LongDescription ?? trip.LongDescription,
                MetaTitle: englishTranslation?.MetaTitle ?? trip.MetaTitle,
                MetaDescription: englishTranslation?.MetaDescription ?? trip.MetaDescription,
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
                    .Select(i => new AdminTripItineraryItemDto(
                        i.Id,
                        i.DisplayOrder,
                        i.Translations?
                            .Select(t => new AdminItineraryItemTranslationDto(
                                t.Id,
                                t.Language,
                                t.Title,
                                t.Description))
                            .ToList() ?? new List<AdminItineraryItemTranslationDto>()))
                    .ToList() ?? new List<AdminTripItineraryItemDto>(),
                Includes: trip.Includes?
                    .Select(i => new AdminTripIncludeDto(
                        i.Id,
                        i.Translations?
                            .Select(t => new AdminIncludeTranslationDto(
                                t.Id,
                                t.Language,
                                t.Description))
                            .ToList() ?? new List<AdminIncludeTranslationDto>()))
                    .ToList() ?? new List<AdminTripIncludeDto>(),
                Excludes: trip.Excludes?
                    .Select(e => new AdminTripExcludeDto(
                        e.Id,
                        e.Translations?
                            .Select(t => new AdminExcludeTranslationDto(
                                t.Id,
                                t.Language,
                                t.Description))
                            .ToList() ?? new List<AdminExcludeTranslationDto>()))
                    .ToList() ?? new List<AdminTripExcludeDto>(),
                Highlights: trip.Highlights?
                    .OrderBy(h => h.DisplayOrder)
                    .Select(h => new AdminTripHighlightDto(
                        h.Id,
                        h.DisplayOrder,
                        h.Translations?
                            .Select(t => new AdminHighlightTranslationDto(
                                t.Id,
                                t.Language,
                                t.Description))
                            .ToList() ?? new List<AdminHighlightTranslationDto>()))
                    .ToList() ?? new List<AdminTripHighlightDto>(),
                WhatToBringItems: trip.WhatToBringItems?
                    .OrderBy(w => w.DisplayOrder)
                    .Select(w => new AdminTripWhatToBringDto(
                        w.Id,
                        w.DisplayOrder,
                        w.Translations?
                            .Select(t => new AdminWhatToBringTranslationDto(
                                t.Id,
                                t.Language,
                                t.Description))
                            .ToList() ?? new List<AdminWhatToBringTranslationDto>()))
                    .ToList() ?? new List<AdminTripWhatToBringDto>(),
                FAQs: trip.FAQs?
                    .OrderBy(f => f.DisplayOrder)
                    .Select(f => new AdminTripFAQDto(
                        f.Id,
                        f.Question,
                        f.Answer,
                        f.DisplayOrder,
                        f.IsActive,
                        f.Translations?
                            .Select(t => new AdminFAQTranslationDto(
                                t.Id,
                                t.Language,
                                t.Question,
                                t.Answer))
                            .ToList() ?? new List<AdminFAQTranslationDto>()))
                    .ToList() ?? new List<AdminTripFAQDto>(),
                Translations: trip.Translations?
                    .Select(t => new AdminTripTranslationDto(
                        t.Id,
                        t.Language,
                        t.Title,
                        t.ShortDescription,
                        t.LongDescription,
                        t.MetaTitle,
                        t.MetaDescription,
                        t.PickupLocation))
                    .ToList() ?? new List<AdminTripTranslationDto>()
            );
        }
    }
}
