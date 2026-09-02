using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Storage;
using Application.Interfaces.Trips;
using Domain.Entity;
using Domain.Entitys;
using Domain.Enum;
using FluentValidation;

namespace Application.Services.Trips
{
    public class UpdateTripService : IUpdateTripService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorage;
        private readonly IValidator<UpdateTripDto> _validator;

        public UpdateTripService(
            IUnitOfWork unitOfWork,
            IFileStorageService fileStorage,
            IValidator<UpdateTripDto> validator)
        {
            _unitOfWork = unitOfWork;
            _fileStorage = fileStorage;
            _validator = validator;
        }

        public async Task<TripUpdatedResponseDto> UpdateTripAsync(
            Guid id,
            UpdateTripDto dto,
            CancellationToken cancellationToken = default)
        {
            // 1. Validate DTO
            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            // 2. Validate related entity existence
            if (!await _unitOfWork.Categories.ExistsAndIsActiveAsync(dto.CategoryId, cancellationToken))
            {
                throw new ArgumentException("Category does not exist or is inactive.");
            }

            if (!await _unitOfWork.Destinations.ExistsAndIsActiveAsync(dto.DestinationId, cancellationToken))
            {
                throw new ArgumentException("Destination does not exist or is inactive.");
            }

            if (!await _unitOfWork.TourTypes.ExistsAndIsActiveAsync(dto.TourTypeId, cancellationToken))
            {
                throw new ArgumentException("Tour type does not exist or is inactive.");
            }

            // 3. Validate Slug uniqueness (excluding current trip)
            if (await _unitOfWork.Trips.ExistsBySlugOtherThanIdAsync(dto.Slug, id, cancellationToken))
            {
                throw new InvalidOperationException("Trip slug already exists for another trip.");
            }

            // 4. Load existing Trip with tracking
            var trip = await _unitOfWork.Trips.GetByIdForUpdateAsync(id, cancellationToken);
            if (trip == null)
            {
                throw new KeyNotFoundException($"Trip with ID '{id}' was not found.");
            }

            var newlyUploadedFiles = new List<string>();
            var oldFilesToDelete = new List<string>();

            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                // 5. Handle Cover Image replacement → Convert to TripImage with IsCover=true
                if (dto.CoverImage != null)
                {
                    var newCoverPath = await _fileStorage.SaveAsync(dto.CoverImage, "trips", cancellationToken);
                    newlyUploadedFiles.Add(newCoverPath);

                    // Remove old cover from gallery if it exists
                    var oldCover = trip.Images.FirstOrDefault(i => i.IsCover);
                    if (oldCover != null)
                    {
                        oldFilesToDelete.Add(oldCover.ImageUrl);
                        trip.Images.Remove(oldCover);
                    }

                    // Add new cover as TripImage
                    trip.Images.Add(new TripImage
                    {
                        Id = Guid.NewGuid(),
                        TripId = trip.Id,
                        ImageUrl = newCoverPath,
                        AltText = dto.CoverImageAltText ?? dto.Title,
                        DisplayOrder = -1,
                        IsCover = true,
                        CreatedAt = DateTime.UtcNow
                    });
                }
                else if (dto.CoverImageAltText != null)
                {
                    // Update alt text for existing cover
                    var existingCover = trip.Images.FirstOrDefault(i => i.IsCover);
                    if (existingCover != null)
                    {
                        existingCover.AltText = dto.CoverImageAltText;
                        existingCover.UpdatedAt = DateTime.UtcNow;
                    }
                }

                // 6. Handle Og Image replacement
                if (dto.OgImage != null)
                {
                    var newOgPath = await _fileStorage.SaveAsync(dto.OgImage, "trips", cancellationToken);
                    newlyUploadedFiles.Add(newOgPath);

                    if (!string.IsNullOrEmpty(trip.OgImage))
                    {
                        oldFilesToDelete.Add(trip.OgImage);
                    }

                    trip.OgImage = newOgPath;
                }

                // 7. Handle Gallery Images replacement
                if (dto.GalleryImages != null && dto.GalleryImages.Count > 0)
                {
                    // Remove old non-cover gallery images
                    var oldGalleryImages = trip.Images.Where(i => !i.IsCover).ToList();
                    foreach (var img in oldGalleryImages)
                    {
                        if (!string.IsNullOrEmpty(img.ImageUrl))
                        {
                            oldFilesToDelete.Add(img.ImageUrl);
                        }
                        trip.Images.Remove(img);
                    }

                    // Add new gallery images (starting at DisplayOrder 0)
                    for (int i = 0; i < dto.GalleryImages.Count; i++)
                    {
                        var imageFile = dto.GalleryImages[i];
                        var imagePath = await _fileStorage.SaveAsync(imageFile, "trips", cancellationToken);
                        newlyUploadedFiles.Add(imagePath);

                        trip.Images.Add(new TripImage
                        {
                            Id = Guid.NewGuid(),
                            TripId = trip.Id,
                            ImageUrl = imagePath,
                            AltText = dto.GalleryAltTexts != null && i < dto.GalleryAltTexts.Count ? dto.GalleryAltTexts[i] : dto.Title,
                            DisplayOrder = i,
                            IsCover = false,
                            CreatedAt = DateTime.UtcNow
                        });
                    }
                }

                // 8. Update Scalar Fields
                trip.Title = dto.Title;
                trip.Slug = dto.Slug.ToLowerInvariant();
                trip.Status = dto.Status;
                trip.IsFeatured = dto.IsFeatured;
                trip.DisplayOrder = dto.DisplayOrder;
                trip.Duration = dto.Duration;
                trip.DurationUnit = dto.DurationUnit;
                trip.PickupLocation = dto.PickupLocation;
                trip.Currency = dto.Currency;
                trip.AdultPrice = dto.AdultPrice;
                trip.ChildPrice = dto.ChildPrice;
                trip.OldPrice = dto.OldPrice;
                trip.IsPriceFrom = dto.IsPriceFrom;
                trip.ShortDescription = dto.ShortDescription;
                trip.LongDescription = dto.LongDescription;
                trip.MetaTitle = dto.MetaTitle;
                trip.MetaDescription = dto.MetaDescription;
                trip.CategoryId = dto.CategoryId;
                trip.DestinationId = dto.DestinationId;
                trip.TourTypeId = dto.TourTypeId;
                trip.UpdatedAt = DateTime.UtcNow;

                // 9. Update Itinerary Items
                trip.ItineraryItems.Clear();
                foreach (var item in dto.ItineraryItems)
                {
                    trip.ItineraryItems.Add(new TripItineraryItem
                    {
                        Id = Guid.NewGuid(),
                        TripId = trip.Id,
                        Title = item.Title,
                        Description = item.Description,
                        DisplayOrder = item.DisplayOrder
                    });
                }

                // 10. Update Includes
                trip.Includes.Clear();
                foreach (var item in dto.Includes)
                {
                    trip.Includes.Add(new TripInclude
                    {
                        Id = Guid.NewGuid(),
                        TripId = trip.Id,
                        Description = item.Description
                    });
                }

                // 11. Update Excludes
                trip.Excludes.Clear();
                foreach (var item in dto.Excludes)
                {
                    trip.Excludes.Add(new TripExclude
                    {
                        Id = Guid.NewGuid(),
                        TripId = trip.Id,
                        Description = item.Description
                    });
                }

                // 12. Update FAQs and FAQ Translations
                trip.FAQs.Clear();
                foreach (var faqDto in dto.FAQs)
                {
                    var faq = new FAQ
                    {
                        Id = Guid.NewGuid(),
                        TripId = trip.Id,
                        Question = faqDto.Question,
                        Answer = faqDto.Answer,
                        DisplayOrder = faqDto.DisplayOrder,
                        IsActive = faqDto.IsActive
                    };

                    foreach (var tr in faqDto.Translations)
                    {
                        faq.Translations.Add(new FAQTranslation
                        {
                            Id = Guid.NewGuid(),
                            FAQId = faq.Id,
                            Language = tr.Language,
                            Question = tr.Question,
                            Answer = tr.Answer
                        });
                    }

                    trip.FAQs.Add(faq);
                }

                // 13. Update Trip Translations
                trip.Translations.Clear();
                foreach (var tr in dto.Translations)
                {
                    trip.Translations.Add(new TripTranslation
                    {
                        Id = Guid.NewGuid(),
                        TripId = trip.Id,
                        Language = tr.Language,
                        Title = tr.Title,
                        ShortDescription = tr.ShortDescription,
                        LongDescription = tr.LongDescription,
                        MetaTitle = tr.MetaTitle,
                        MetaDescription = tr.MetaDescription
                    });
                }

                // 14. Save changes and commit transaction
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);

                // 15. Post-commit physical file deletion of old replaced files
                foreach (var oldFile in oldFilesToDelete)
                {
                    await _fileStorage.DeleteAsync(oldFile);
                }

                return new TripUpdatedResponseDto(
                    trip.Id,
                    trip.Slug,
                    "Trip updated successfully.");
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);

                // Clean up newly uploaded files on transaction failure
                foreach (var file in newlyUploadedFiles)
                {
                    await _fileStorage.DeleteAsync(file);
                }

                throw;
            }
        }
    }
}
