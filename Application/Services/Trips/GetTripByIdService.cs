using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Trips;
using Domain.Entity;

namespace Application.Services.Trips
{
    public class GetTripByIdService : IGetTripByIdService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTripByIdService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TripDetailsResponseDto> GetTripByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Trip Id cannot be empty.", nameof(id));
            }

            var trip = await _unitOfWork.Trips.GetByIdWithDetailsAsync(id, cancellationToken);
            if (trip == null)
            {
                throw new KeyNotFoundException($"Trip with ID '{id}' was not found.");
            }

            return MapToDto(trip);
        }

        private static TripDetailsResponseDto MapToDto(Trip trip)
        {
            var coverImage = trip.Images?.FirstOrDefault(i => i.IsCover);

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
                Translation: null 
            );
        }
    }
}
