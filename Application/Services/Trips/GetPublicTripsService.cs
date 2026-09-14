using Application.DTOs.Common;
using Application.DTOs.Trips;
using Application.Interfaces.Repositories;
using Application.Interfaces.Trips;
using Domain.Entity;
using Domain.Enum;
using FluentValidation;

namespace Application.Services.Trips
{
    public class GetPublicTripsService : IGetPublicTripsService
    {
        private readonly ITripRepository _tripRepository;
        private readonly IValidator<GetPublicTripsQueryDto> _validator;

        public GetPublicTripsService(
            ITripRepository tripRepository,
            IValidator<GetPublicTripsQueryDto> validator)
        {
            _tripRepository = tripRepository;
            _validator = validator;
        }

        public async Task<PagedResult<PublicTripSummaryDto>> GetPublicTripsAsync(GetPublicTripsQueryDto query,CancellationToken cancellationToken = default)
        {
            await _validator.ValidateAndThrowAsync(query,cancellationToken);

            var (items, totalCount) = await _tripRepository.GetPublicTripsAsync(query, cancellationToken);

            var dtos = items
                .Select(trip => MapToSummaryDto(trip, query.Language))
                .ToList();

            return new PagedResult<PublicTripSummaryDto>(
                dtos,
                query.Page,
                query.PageSize,
                totalCount
            );
        }

        private static PublicTripSummaryDto MapToSummaryDto(Trip trip, Language? language)
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

            return new PublicTripSummaryDto(
                Id: trip.Id,
                Title: translation.Title,
                Slug: trip.Slug,
                ShortDescription: translation.ShortDescription,
                Status: trip.Status,
                IsFeatured: trip.IsFeatured,
                DisplayOrder: trip.DisplayOrder,
                Duration: trip.Duration,
                DurationUnit: trip.DurationUnit,
                Currency: trip.Currency,
                AdultPrice: trip.AdultPrice,
                ChildPrice: trip.ChildPrice,
                OldPrice: trip.OldPrice,
                IsPriceFrom: trip.IsPriceFrom,
                CoverImage: coverImage?.ImageUrl,
                CoverImageAltText: coverImage?.AltText,
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
                )
            );
        }
    }
}
