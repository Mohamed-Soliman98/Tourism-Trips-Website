using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.Trips;
using Domain.Entity;
using Domain.Entitys;
using Domain.Enum;
using FluentValidation;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Trips
{
    public class CreateTripService : ICreateTripService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDestinationRepository _destinationRepository;
        private readonly ITourTypeRepository _tourTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateTripDto> _validator;
        public CreateTripService(
            IUnitOfWork unitOfWork,
            ITourTypeRepository tourTypeRepository,
            IDestinationRepository destinationRepository,
            ICategoryRepository categoryRepository,
            ITripRepository tripRepository,
            IValidator<CreateTripDto> validator)
        {
            _unitOfWork = unitOfWork;
            _tourTypeRepository = tourTypeRepository;
            _destinationRepository = destinationRepository;
            _categoryRepository = categoryRepository;
            _tripRepository = tripRepository;
            _validator = validator;
        }

        public async Task<TripCreatedResponseDto> CreateTripAsync( CreateTripDto dto, CancellationToken cancellationToken = default)
        {
            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            if (!await _categoryRepository.ExistsAndIsActiveAsync(dto.CategoryId, cancellationToken))
            {
                throw new ArgumentException("Category does not exist or is inactive.");
            }

            if (!await _destinationRepository.ExistsAndIsActiveAsync(dto.DestinationId, cancellationToken))
            {
                throw new ArgumentException("Destination does not exist or is inactive.");
            }

            if (!await _tourTypeRepository.ExistsAndIsActiveAsync(dto.TourTypeId, cancellationToken))
            {
                throw new ArgumentException("Tour type does not exist or is inactive.");
            }

            if (await _tripRepository.ExistsBySlugAsync(dto.Slug, cancellationToken))
            {
                throw new InvalidOperationException("Trip slug already exists.");
            }

            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var trip = new Trip
                {
                    Id = Guid.NewGuid(),
                    Title = dto.Title.English?.Trim() ?? string.Empty,
                    Slug = dto.Slug.ToLowerInvariant().Trim(),
                    Status = TripStatus.Draft,
                    IsFeatured = dto.IsFeatured,
                    DisplayOrder = dto.DisplayOrder,
                    Duration = dto.Duration,
                    DurationUnit = dto.DurationUnit,
                    PickupLocation = dto.PickupLocation?.English?.Trim(),
                    Currency = dto.Currency,
                    AdultPrice = dto.AdultPrice,
                    ChildPrice = dto.ChildPrice,
                    OldPrice = dto.OldPrice,
                    IsPriceFrom = dto.IsPriceFrom,
                    ShortDescription = dto.ShortDescription.English?.Trim() ?? string.Empty,
                    LongDescription = dto.LongDescription.English?.Trim() ?? string.Empty,
                    MetaTitle = dto.MetaTitle?.English?.Trim(),
                    MetaDescription = dto.MetaDescription?.English?.Trim(),
                    Notes = dto.Notes,
                    CategoryId = dto.CategoryId,
                    DestinationId = dto.DestinationId,
                    TourTypeId = dto.TourTypeId
                };

                // Create Trip translations for all supported languages
                foreach (var language in TranslationHelper.GetLanguagesToCreate(
                    dto.Title, dto.ShortDescription, dto.LongDescription, dto.MetaTitle, dto.MetaDescription, dto.PickupLocation))
                {
                    trip.Translations.Add(new TripTranslation
                    {
                        Id = Guid.NewGuid(),
                        TripId = trip.Id,
                        Language = language,
                        Title = TranslationHelper.GetValueOrEnglishFallback(dto.Title, language, isRequired: true),
                        ShortDescription = TranslationHelper.GetValueOrEnglishFallback(dto.ShortDescription, language, isRequired: true),
                        LongDescription = TranslationHelper.GetValueOrEnglishFallback(dto.LongDescription, language, isRequired: true),
                        MetaTitle = TranslationHelper.GetOptionalValueOrEnglishFallback(dto.MetaTitle, language),
                        MetaDescription = TranslationHelper.GetOptionalValueOrEnglishFallback(dto.MetaDescription, language),
                        PickupLocation = TranslationHelper.GetOptionalValueOrEnglishFallback(dto.PickupLocation, language)
                    });
                }

                // Itinerary Items
                foreach (var item in dto.ItineraryItems)
                {
                    var itineraryItem = new TripItineraryItem
                    {
                        Id = Guid.NewGuid(),
                        TripId = trip.Id,
                        DisplayOrder = item.DisplayOrder
                    };

                    foreach (var language in TranslationHelper.GetLanguagesToCreate(item.Title, item.Description))
                    {
                        itineraryItem.Translations.Add(new TripItineraryItemTranslation
                        {
                            Id = Guid.NewGuid(),
                            TripItineraryItemId = itineraryItem.Id,
                            Language = language,
                            Title = TranslationHelper.GetValueOrEnglishFallback(item.Title, language, isRequired: true),
                            Description = TranslationHelper.GetOptionalValueOrEnglishFallback(item.Description, language)
                        });
                    }

                    trip.ItineraryItems.Add(itineraryItem);
                }

                // Includes
                foreach (var item in dto.Includes)
                {
                    var include = new TripInclude
                    {
                        Id = Guid.NewGuid(),
                        TripId = trip.Id
                    };

                    foreach (var language in TranslationHelper.GetLanguagesToCreate(item.Description))
                    {
                        include.Translations.Add(new TripIncludeTranslation
                        {
                            Id = Guid.NewGuid(),
                            TripIncludeId = include.Id,
                            Language = language,
                            Description = TranslationHelper.GetValueOrEnglishFallback(item.Description, language, isRequired: true)
                        });
                    }

                    trip.Includes.Add(include);
                }

                // Excludes
                foreach (var item in dto.Excludes)
                {
                    var exclude = new TripExclude
                    {
                        Id = Guid.NewGuid(),
                        TripId = trip.Id
                    };

                    foreach (var language in TranslationHelper.GetLanguagesToCreate(item.Description))
                    {
                        exclude.Translations.Add(new TripExcludeTranslation
                        {
                            Id = Guid.NewGuid(),
                            TripExcludeId = exclude.Id,
                            Language = language,
                            Description = TranslationHelper.GetValueOrEnglishFallback(item.Description, language, isRequired: true)
                        });
                    }

                    trip.Excludes.Add(exclude);
                }

                // Highlights
                foreach (var item in dto.Highlights)
                {
                    var highlight = new TripHighlight
                    {
                        Id = Guid.NewGuid(),
                        TripId = trip.Id,
                        DisplayOrder = item.DisplayOrder
                    };

                    foreach (var language in TranslationHelper.GetLanguagesToCreate(item.Description))
                    {
                        highlight.Translations.Add(new TripHighlightTranslation
                        {
                            Id = Guid.NewGuid(),
                            TripHighlightId = highlight.Id,
                            Language = language,
                            Description = TranslationHelper.GetValueOrEnglishFallback(item.Description, language, isRequired: true)
                        });
                    }

                    trip.Highlights.Add(highlight);
                }

                // What To Bring Items
                foreach (var item in dto.WhatToBringItems)
                {
                    var whatToBring = new TripWhatToBring
                    {
                        Id = Guid.NewGuid(),
                        TripId = trip.Id,
                        DisplayOrder = item.DisplayOrder
                    };

                    foreach (var language in TranslationHelper.GetLanguagesToCreate(item.Description))
                    {
                        whatToBring.Translations.Add(new TripWhatToBringTranslation
                        {
                            Id = Guid.NewGuid(),
                            TripWhatToBringId = whatToBring.Id,
                            Language = language,
                            Description = TranslationHelper.GetValueOrEnglishFallback(item.Description, language, isRequired: true)
                        });
                    }

                    trip.WhatToBringItems.Add(whatToBring);
                }

                // FAQs
                foreach (var faqDto in dto.FAQs)
                {
                    var faq = new FAQ
                    {
                        Id = Guid.NewGuid(),
                        TripId = trip.Id,
                        Question = faqDto.Question.English?.Trim() ?? string.Empty,
                        Answer = faqDto.Answer.English?.Trim() ?? string.Empty,
                        DisplayOrder = faqDto.DisplayOrder,
                        IsActive = faqDto.IsActive
                    };

                    foreach (var language in TranslationHelper.GetLanguagesToCreate(faqDto.Question, faqDto.Answer))
                    {
                        faq.Translations.Add(new FAQTranslation
                        {
                            Id = Guid.NewGuid(),
                            FAQId = faq.Id,
                            Language = language,
                            Question = TranslationHelper.GetValueOrEnglishFallback(faqDto.Question, language, isRequired: true),
                            Answer = TranslationHelper.GetValueOrEnglishFallback(faqDto.Answer, language, isRequired: true)
                        });
                    }

                    trip.FAQs.Add(faq);
                }

                _tripRepository.Add(trip);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return new TripCreatedResponseDto(trip.Id, trip.Slug, "Trip created successfully as Draft.");
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }
    }
}
