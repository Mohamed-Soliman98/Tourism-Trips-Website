using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.Trips;
using Domain.Entity;
using Domain.Entitys;
using Domain.Enum;

namespace Application.Services.Trips
{
    public class DuplicateTripService : IDuplicateTripService
    {
        private readonly ITripRepository _tripRepository;
        private readonly ITripTranslationRepository _tripTranslationRepository;
        private readonly ITripImageRepository _tripImageRepository;
        private readonly ITripItineraryItemRepository _tripItineraryItemRepository;
        private readonly ITripIncludeRepository _tripIncludeRepository;
        private readonly ITripExcludeRepository _tripExcludeRepository;
        private readonly IFAQRepository _faqRepository;
        private readonly IFAQTranslationRepository _faqTranslationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DuplicateTripService(
            IUnitOfWork unitOfWork,
            ITripRepository tripRepository,
            ITripTranslationRepository tripTranslationRepository,
            ITripImageRepository tripImageRepository,
            ITripItineraryItemRepository tripItineraryItemRepository,
            ITripIncludeRepository tripIncludeRepository,
            ITripExcludeRepository tripExcludeRepository,
            IFAQRepository faqRepository,
            IFAQTranslationRepository faqTranslationRepository)
        {
            _unitOfWork = unitOfWork;
            _tripRepository = tripRepository;
            _tripTranslationRepository = tripTranslationRepository;
            _tripImageRepository = tripImageRepository;
            _tripItineraryItemRepository = tripItineraryItemRepository;
            _tripIncludeRepository = tripIncludeRepository;
            _tripExcludeRepository = tripExcludeRepository;
            _faqRepository = faqRepository;
            _faqTranslationRepository = faqTranslationRepository;
        }

        public async Task<TripDuplicatedResponseDto> DuplicateTripAsync(Guid tripId, CancellationToken cancellationToken)
        {
            var originalTrip = await _tripRepository.GetByIdWithDetailsAsync(tripId, cancellationToken);

            if (originalTrip == null)
            {
                throw new KeyNotFoundException($"Trip with ID {tripId} not found.");
            }

            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var newSlug = await GenerateUniqueSlugAsync(originalTrip.Slug, cancellationToken);

                var newTrip = new Trip
                {
                    Id = Guid.NewGuid(),
                    Title = originalTrip.Title,
                    Slug = newSlug,
                    Status = TripStatus.Draft,
                    IsFeatured = originalTrip.IsFeatured,
                    DisplayOrder = originalTrip.DisplayOrder,
                    Duration = originalTrip.Duration,
                    DurationUnit = originalTrip.DurationUnit,
                    PickupLocation = originalTrip.PickupLocation,
                    Currency = originalTrip.Currency,
                    AdultPrice = originalTrip.AdultPrice,
                    ChildPrice = originalTrip.ChildPrice,
                    OldPrice = originalTrip.OldPrice,
                    IsPriceFrom = originalTrip.IsPriceFrom,
                    ShortDescription = originalTrip.ShortDescription,
                    LongDescription = originalTrip.LongDescription,
                    MetaTitle = originalTrip.MetaTitle,
                    MetaDescription = originalTrip.MetaDescription,
                    OgImage = originalTrip.OgImage,
                    Notes = originalTrip.Notes,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                    CategoryId = originalTrip.CategoryId,
                    DestinationId = originalTrip.DestinationId,
                    TourTypeId = originalTrip.TourTypeId
                };

                _tripRepository.Add(newTrip);

                foreach (var translation in originalTrip.Translations)
                {
                    var newTranslation = new TripTranslation
                    {
                        Id = Guid.NewGuid(),
                        Language = translation.Language,
                        Title = translation.Title,
                        ShortDescription = translation.ShortDescription,
                        LongDescription = translation.LongDescription,
                        MetaTitle = translation.MetaTitle,
                        MetaDescription = translation.MetaDescription,
                        PickupLocation = translation.PickupLocation,
                        TripId = newTrip.Id
                    };

                    _tripTranslationRepository.Add(newTranslation);
                }

                foreach (var image in originalTrip.Images)
                {
                    var newImage = new TripImage
                    {
                        Id = Guid.NewGuid(),
                        ImageUrl = image.ImageUrl,
                        AltText = image.AltText,
                        DisplayOrder = image.DisplayOrder,
                        IsCover = image.IsCover,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = null,
                        TripId = newTrip.Id
                    };

                    _tripImageRepository.Add(newImage);
                }

                foreach (var itineraryItem in originalTrip.ItineraryItems)
                {
                    var newItineraryItem = new TripItineraryItem
                    {
                        Id = Guid.NewGuid(),
                        DisplayOrder = itineraryItem.DisplayOrder,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = null,
                        TripId = newTrip.Id
                    };

                    foreach (var tr in itineraryItem.Translations)
                    {
                        newItineraryItem.Translations.Add(new TripItineraryItemTranslation
                        {
                            Id = Guid.NewGuid(),
                            TripItineraryItemId = newItineraryItem.Id,
                            Language = tr.Language,
                            Title = tr.Title,
                            Description = tr.Description
                        });
                    }

                    _tripItineraryItemRepository.Add(newItineraryItem);
                }

                foreach (var include in originalTrip.Includes)
                {
                    var newInclude = new TripInclude
                    {
                        Id = Guid.NewGuid(),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = null,
                        TripId = newTrip.Id
                    };

                    foreach (var tr in include.Translations)
                    {
                        newInclude.Translations.Add(new TripIncludeTranslation
                        {
                            Id = Guid.NewGuid(),
                            TripIncludeId = newInclude.Id,
                            Language = tr.Language,
                            Description = tr.Description
                        });
                    }

                    _tripIncludeRepository.Add(newInclude);
                }

                foreach (var exclude in originalTrip.Excludes)
                {
                    var newExclude = new TripExclude
                    {
                        Id = Guid.NewGuid(),
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = null,
                        TripId = newTrip.Id
                    };

                    foreach (var tr in exclude.Translations)
                    {
                        newExclude.Translations.Add(new TripExcludeTranslation
                        {
                            Id = Guid.NewGuid(),
                            TripExcludeId = newExclude.Id,
                            Language = tr.Language,
                            Description = tr.Description
                        });
                    }

                    _tripExcludeRepository.Add(newExclude);
                }

                foreach (var highlight in originalTrip.Highlights)
                {
                    var newHighlight = new TripHighlight
                    {
                        Id = Guid.NewGuid(),
                        DisplayOrder = highlight.DisplayOrder,
                        TripId = newTrip.Id
                    };

                    foreach (var tr in highlight.Translations)
                    {
                        newHighlight.Translations.Add(new TripHighlightTranslation
                        {
                            Id = Guid.NewGuid(),
                            TripHighlightId = newHighlight.Id,
                            Language = tr.Language,
                            Description = tr.Description
                        });
                    }

                    newTrip.Highlights.Add(newHighlight);
                }

                foreach (var item in originalTrip.WhatToBringItems)
                {
                    var newWhatToBring = new TripWhatToBring
                    {
                        Id = Guid.NewGuid(),
                        DisplayOrder = item.DisplayOrder,
                        TripId = newTrip.Id
                    };

                    foreach (var tr in item.Translations)
                    {
                        newWhatToBring.Translations.Add(new TripWhatToBringTranslation
                        {
                            Id = Guid.NewGuid(),
                            TripWhatToBringId = newWhatToBring.Id,
                            Language = tr.Language,
                            Description = tr.Description
                        });
                    }

                    newTrip.WhatToBringItems.Add(newWhatToBring);
                }

                foreach (var faq in originalTrip.FAQs)
                {
                    var newFaq = new FAQ
                    {
                        Id = Guid.NewGuid(),
                        Question = faq.Question,
                        Answer = faq.Answer,
                        DisplayOrder = faq.DisplayOrder,
                        IsActive = faq.IsActive,
                        TripId = newTrip.Id,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = null
                    };

                    _faqRepository.Add(newFaq);

                    foreach (var faqTranslation in faq.Translations)
                    {
                        var newFaqTranslation = new FAQTranslation
                        {
                            Id = Guid.NewGuid(),
                            Language = faqTranslation.Language,
                            Question = faqTranslation.Question,
                            Answer = faqTranslation.Answer,
                            FAQId = newFaq.Id
                        };

                        _faqTranslationRepository.Add(newFaqTranslation);
                    }
                }

                _tripRepository.Add(newTrip);

                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                return new TripDuplicatedResponseDto(
                    newTrip.Id,
                    newTrip.Title,
                    newTrip.Slug,
                    newTrip.Status,
                    newTrip.CreatedAt
                );
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }
        }

        private async Task<string> GenerateUniqueSlugAsync(string originalSlug, CancellationToken cancellationToken)
        {
            var baseSlug = originalSlug;
            var counter = 1;
            var newSlug = $"{baseSlug}-copy";

            while (await _tripRepository.ExistsBySlugAsync(newSlug, cancellationToken))
            {
                counter++;
                newSlug = $"{baseSlug}-copy-{counter}";
            }

            return newSlug;
        }
    }
}