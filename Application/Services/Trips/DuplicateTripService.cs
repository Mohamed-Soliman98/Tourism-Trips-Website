using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Trips;
using Domain.Entity;
using Domain.Entitys;
using Domain.Enum;

namespace Application.Services.Trips
{
    public class DuplicateTripService : IDuplicateTripService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DuplicateTripService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TripDuplicatedResponseDto> DuplicateTripAsync(Guid tripId, CancellationToken cancellationToken)
        {
            // Find the original trip with all related data
            var originalTrip = await _unitOfWork.Trips.GetByIdWithDetailsAsync(tripId, cancellationToken);
            
            if (originalTrip == null)
            {
                throw new KeyNotFoundException($"Trip with ID {tripId} not found.");
            }

            // Begin transaction for atomic operation
            using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            
            try
            {
                // Generate unique slug for the duplicated trip
                var newSlug = await GenerateUniqueSlugAsync(originalTrip.Slug, cancellationToken);

                // Create the new trip
                var newTrip = new Trip
                {
                    Id = Guid.NewGuid(),
                    Title = originalTrip.Title,
                    Slug = newSlug,
                    Status = TripStatus.Draft, // Always start as Draft
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
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = null,
                    CategoryId = originalTrip.CategoryId,
                    DestinationId = originalTrip.DestinationId,
                    TourTypeId = originalTrip.TourTypeId
                };

                _unitOfWork.Trips.Add(newTrip);

                // Duplicate translations
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
                        TripId = newTrip.Id
                    };
                    
                    _unitOfWork.TripTranslations.Add(newTranslation);
                }

                // Duplicate images
                foreach (var image in originalTrip.Images)
                {
                    var newImage = new TripImage
                    {
                        Id = Guid.NewGuid(),
                        ImageUrl = image.ImageUrl,
                        AltText = image.AltText,
                        DisplayOrder = image.DisplayOrder,
                        IsCover = image.IsCover, // Can be copied since it's a new trip
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = null,
                        TripId = newTrip.Id
                    };
                    
                    _unitOfWork.TripImages.Add(newImage);
                }

                // Duplicate itinerary items
                foreach (var itineraryItem in originalTrip.ItineraryItems)
                {
                    var newItineraryItem = new TripItineraryItem
                    {
                        Id = Guid.NewGuid(),
                        DisplayOrder = itineraryItem.DisplayOrder,
                        Title = itineraryItem.Title,
                        Description = itineraryItem.Description,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = null,
                        TripId = newTrip.Id
                    };
                    
                    _unitOfWork.TripItineraryItems.Add(newItineraryItem);
                }

                // Duplicate includes
                foreach (var include in originalTrip.Includes)
                {
                    var newInclude = new TripInclude
                    {
                        Id = Guid.NewGuid(),
                        Description = include.Description,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = null,
                        TripId = newTrip.Id
                    };
                    
                    _unitOfWork.TripIncludes.Add(newInclude);
                }

                // Duplicate excludes
                foreach (var exclude in originalTrip.Excludes)
                {
                    var newExclude = new TripExclude
                    {
                        Id = Guid.NewGuid(),
                        Description = exclude.Description,
                        CreatedAt = DateTime.UtcNow,
                        UpdatedAt = null,
                        TripId = newTrip.Id
                    };
                    
                    _unitOfWork.TripExcludes.Add(newExclude);
                }

                // Duplicate FAQs
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
                    
                    _unitOfWork.FAQs.Add(newFaq);

                    // Duplicate FAQ translations
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
                        
                        _unitOfWork.FAQTranslations.Add(newFaqTranslation);
                    }
                }

                // Save all changes
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

            // Keep generating unique slugs until we find one that doesn't exist
            while (await _unitOfWork.Trips.ExistsBySlugAsync(newSlug, cancellationToken))
            {
                counter++;
                newSlug = $"{baseSlug}-copy-{counter}";
            }

            return newSlug;
        }
    }
}