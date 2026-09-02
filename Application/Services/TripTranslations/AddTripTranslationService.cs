using Application.DTOs.TripTranslations;
using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.TripTranslations;
using Domain.Entitys;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.TripTranslations
{
    public class AddTripTranslationService : IAddTripTranslationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateTripTranslationDto> _validator;

        public AddTripTranslationService(
            IUnitOfWork unitOfWork,
            IValidator<CreateTripTranslationDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<TripTranslationAddedResponseDto> AddTripTranslationAsync(
            Guid tripId,
            CreateTripTranslationDto dto,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            var trip = await _unitOfWork.Trips.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            if (await _unitOfWork.TripTranslations.ExistsByTripIdAndLanguageAsync(
                    tripId, dto.Language, cancellationToken))
            {
                throw new InvalidOperationException(
                    $"A translation for language '{dto.Language}' already exists for trip '{tripId}'.");
            }

            var translation = new TripTranslation
            {
                Id = Guid.NewGuid(),
                TripId = tripId,
                Language = dto.Language,
                Title = dto.Title.Trim(),
                ShortDescription = dto.ShortDescription.Trim(),
                LongDescription = dto.LongDescription.Trim(),
                MetaTitle = dto.MetaTitle?.Trim(),
                MetaDescription = dto.MetaDescription?.Trim()
            };

            _unitOfWork.TripTranslations.Add(translation);

            try
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex) when (TripTranslationUniqueConstraint.IsViolation(ex))
            {
                throw new InvalidOperationException(
                    $"A translation for language '{dto.Language}' already exists for trip '{tripId}'.");
            }

            return new TripTranslationAddedResponseDto(
                translation.Id,
                translation.TripId,
                translation.Language,
                translation.Title,
                translation.ShortDescription,
                translation.LongDescription,
                translation.MetaTitle,
                translation.MetaDescription);
        }
    }
}
