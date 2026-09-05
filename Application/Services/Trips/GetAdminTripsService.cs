using Application.DTOs.Common;
using Application.DTOs.Trips;
using Application.Interfaces.Repositories;
using Application.Interfaces.Trips;
using Domain.Entity;
using FluentValidation;

namespace Application.Services.Trips
{
    public class GetAdminTripsService : IGetAdminTripsService
    {
        private readonly ITripRepository _tripRepository;
        private readonly IValidator<GetAdminTripsQueryDto> _validator;

        public GetAdminTripsService(
            ITripRepository tripRepository,
            IValidator<GetAdminTripsQueryDto> validator)
        {
            _tripRepository = tripRepository;
            _validator = validator;
        }

        public async Task<PagedResult<AdminTripSummaryDto>> GetAdminTripsAsync(
            GetAdminTripsQueryDto query,
            CancellationToken cancellationToken = default)
        {
            await _validator.ValidateAndThrowAsync(query, cancellationToken);

            var (items, totalCount) = await _tripRepository.GetAdminTripsAsync(query, cancellationToken);

            var dtos = items.Select(MapToAdminSummaryDto).ToList();

            return new PagedResult<AdminTripSummaryDto>(
                dtos,
                query.Page,
                query.PageSize,
                totalCount
            );
        }

        private static AdminTripSummaryDto MapToAdminSummaryDto(Trip trip)
        {
            var coverImage = trip.Images?.FirstOrDefault(i => i.IsCover);

            return new AdminTripSummaryDto(
                Id: trip.Id,
                Title: trip.Title,
                Slug: trip.Slug,
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
                ),
                CreatedAt: trip.CreatedAt,
                UpdatedAt: trip.UpdatedAt
            );
        }
    }
}
