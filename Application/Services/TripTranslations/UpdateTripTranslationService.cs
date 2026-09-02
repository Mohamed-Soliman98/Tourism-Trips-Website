using Application.DTOs.TripTranslations;
using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.TripTranslations;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Application.Services.TripTranslations
{
    public class UpdateTripTranslationService : IUpdateTripTranslationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateTripTranslationDto> _validator;

        public UpdateTripTranslationService(
            IUnitOfWork unitOfWork,
            IValidator<UpdateTripTranslationDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<TripTranslationUpdatedResponseDto> UpdateTripTranslationAsync(
            Guid tripId,
            Guid translationId,
            UpdateTripTranslationDto dto,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            if (translationId == Guid.Empty)
                throw new ArgumentException("Translation Id cannot be empty.", nameof(translationId));

            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            var trip = await _unitOfWork.Trips.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var translation = await _unitOfWork.TripTranslations.GetByIdAsync(translationId, cancellationToken);
            if (translation == null)
                throw new KeyNotFoundException($"Trip translation with ID '{translationId}' was not found.");

            if (translation.TripId != tripId)
                throw new InvalidOperationException(
                    $"Trip translation '{translationId}' does not belong to trip '{tripId}'.");

            if (translation.Language != dto.Language
                && await _unitOfWork.TripTranslations.ExistsByTripIdAndLanguageExcludingIdAsync(
                    tripId, dto.Language, translationId, cancellationToken))
            {
                throw new InvalidOperationException(
                    $"A translation for language '{dto.Language}' already exists for trip '{tripId}'.");
            }

            translation.Language = dto.Language;
            translation.Title = dto.Title.Trim();
            translation.ShortDescription = dto.ShortDescription.Trim();
            translation.LongDescription = dto.LongDescription.Trim();
            translation.MetaTitle = dto.MetaTitle?.Trim();
            translation.MetaDescription = dto.MetaDescription?.Trim();

            try
            {
                await _unitOfWork.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateException ex) when (TripTranslationUniqueConstraint.IsViolation(ex))
            {
                throw new InvalidOperationException(
                    $"A translation for language '{dto.Language}' already exists for trip '{tripId}'.");
            }

            return new TripTranslationUpdatedResponseDto(
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
