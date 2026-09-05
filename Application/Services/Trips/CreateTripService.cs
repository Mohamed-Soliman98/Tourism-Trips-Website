using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.Storage;
using Application.Interfaces.Trips;
using Domain.Entity;
using Domain.Entitys;
using Domain.Enum;
using FluentValidation;

namespace Application.Services.Trips
{
    public class CreateTripService : ICreateTripService
    {
     

        private readonly ITripRepository _tripRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IDestinationRepository _destinationRepository;
        private readonly ITourTypeRepository _tourTypeRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IFileStorageService _fileStorage;
        private readonly IValidator<CreateTripDto> _validator;

        public CreateTripService(
            IUnitOfWork unitOfWork,
            IFileStorageService fileStorage,
            ITourTypeRepository tourTypeRepository,
            IDestinationRepository destinationRepository,
            ICategoryRepository categoryRepository,
            ITripRepository tripRepository,
            IValidator<CreateTripDto> validator)
        {
            _unitOfWork = unitOfWork;
            _fileStorage = fileStorage;
            _tourTypeRepository = tourTypeRepository;
            _destinationRepository = destinationRepository;
            _categoryRepository = categoryRepository;
            _tripRepository = tripRepository;
            _validator = validator;
        }

        public async Task<TripCreatedResponseDto> CreateTripAsync(
            CreateTripDto dto,
            CancellationToken cancellationToken = default)
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

            var uploadedFiles = new List<string>();

            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                var trip = new Trip
                {
                    Id = Guid.NewGuid(),
                    Title = dto.Title,
                    Slug = dto.Slug.ToLowerInvariant(),

                    Status = TripStatus.Draft,

                    IsFeatured = dto.IsFeatured,
                    DisplayOrder = dto.DisplayOrder,

                    Duration = dto.Duration,
                    DurationUnit = dto.DurationUnit,
                    PickupLocation = dto.PickupLocation,

                    Currency = dto.Currency,
                    AdultPrice = dto.AdultPrice,
                    ChildPrice = dto.ChildPrice,
                    OldPrice = dto.OldPrice,
                    IsPriceFrom = dto.IsPriceFrom,

                    ShortDescription = dto.ShortDescription,
                    LongDescription = dto.LongDescription,

                    MetaTitle = dto.MetaTitle,
                    MetaDescription = dto.MetaDescription,

                    CategoryId = dto.CategoryId,
                    DestinationId = dto.DestinationId,
                    TourTypeId = dto.TourTypeId
                };

                if (dto.CoverImage is not null)
                {
                    var coverPath = await _fileStorage.SaveAsync(dto.CoverImage, "trips", cancellationToken);
                    uploadedFiles.Add(coverPath);

                    trip.Images.Add(new TripImage
                    {
                        Id = Guid.NewGuid(),
                        TripId = trip.Id,
                        ImageUrl = coverPath,
                        AltText = dto.CoverImageAltText ?? dto.Title,
                        DisplayOrder = -1,
                        IsCover = true,
                        CreatedAt = DateTime.UtcNow
                    });
                }

                int galleryStartOrder = 0;
                for (int i = 0; i < dto.GalleryImages.Count; i++)
                {
                    var image = dto.GalleryImages[i];

                    var imagePath = await _fileStorage.SaveAsync(image, "trips", cancellationToken);

                    uploadedFiles.Add(imagePath);

                    trip.Images.Add(new TripImage
                    {
                        Id = Guid.NewGuid(),
                        TripId = trip.Id,
                        ImageUrl = imagePath,
                        AltText = dto.GalleryAltTexts != null && i < dto.GalleryAltTexts.Count ? dto.GalleryAltTexts[i] : dto.Title,
                        DisplayOrder = galleryStartOrder + i,
                        IsCover = false,
                        CreatedAt = DateTime.UtcNow
                    });
                }

                if (dto.OgImage is not null)
                {
                    var ogImagePath = await _fileStorage.SaveAsync(dto.OgImage, "trips", cancellationToken);

                    uploadedFiles.Add(ogImagePath);

                    trip.OgImage = ogImagePath;
                }

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

                foreach (var item in dto.Includes)
                {
                    trip.Includes.Add(new TripInclude
                    {
                        Id = Guid.NewGuid(),
                        TripId = trip.Id,
                        Description = item.Description
                    });
                }

                foreach (var item in dto.Excludes)
                {
                    trip.Excludes.Add(new TripExclude
                    {
                        Id = Guid.NewGuid(),
                        TripId = trip.Id,
                        Description = item.Description
                    });
                }

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

                    foreach (var translation in faqDto.Translations)
                    {
                        faq.Translations.Add(new FAQTranslation
                        {
                            Id = Guid.NewGuid(),
                            FAQId = faq.Id,
                            Language = translation.Language,
                            Question = translation.Question,
                            Answer = translation.Answer
                        });
                    }

                    trip.FAQs.Add(faq);
                }

                foreach (var translation in dto.Translations)
                {
                    trip.Translations.Add(new TripTranslation
                    {
                        Id = Guid.NewGuid(),
                        TripId = trip.Id,
                        Language = translation.Language,
                        Title = translation.Title,
                        ShortDescription = translation.ShortDescription,
                        LongDescription = translation.LongDescription,
                        MetaTitle = translation.MetaTitle,
                        MetaDescription = translation.MetaDescription
                    });
                }

                _tripRepository.Add(trip);
                await _unitOfWork.SaveChangesAsync(cancellationToken);

                await transaction.CommitAsync(cancellationToken);

                return new TripCreatedResponseDto(trip.Id, trip.Slug, "Trip created successfully as Draft.");
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);

                foreach (var file in uploadedFiles)
                {
                    await _fileStorage.DeleteAsync(file);
                }

                throw;
            }
        }
    }
}
