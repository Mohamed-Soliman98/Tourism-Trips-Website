using Application.DTOs.TripFAQs;
using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripFAQs;
using Domain.Entitys;
using Domain.Enum;
using FluentValidation;

namespace Application.Services.TripFAQs
{
    public class UpdateTripFAQService : IUpdateTripFAQService
    {
        private readonly ITripRepository _tripRepository;
        private readonly IFAQRepository _faqRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateTripFAQDto> _validator;

        public UpdateTripFAQService(
            IUnitOfWork unitOfWork,
            ITripRepository tripRepository,
            IFAQRepository faqRepository,
            IValidator<UpdateTripFAQDto> validator)
        {
            _unitOfWork = unitOfWork;
            _tripRepository = tripRepository;
            _faqRepository = faqRepository;
            _validator = validator;
        }

        public async Task<TripFAQUpdatedResponseDto> UpdateTripFAQAsync(
            Guid tripId,
            Guid faqId,
            UpdateTripFAQDto dto,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            if (faqId == Guid.Empty)
                throw new ArgumentException("FAQ Id cannot be empty.", nameof(faqId));

            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var faq = await _faqRepository.GetByIdAsync(faqId, cancellationToken);
            if (faq == null)
                throw new KeyNotFoundException($"Trip FAQ with ID '{faqId}' was not found.");

            if (faq.TripId != tripId)
                throw new InvalidOperationException(
                    $"Trip FAQ '{faqId}' does not belong to trip '{tripId}'.");

            faq.Question = dto.Question.English?.Trim() ?? string.Empty;
            faq.Answer = dto.Answer.English?.Trim() ?? string.Empty;
            faq.DisplayOrder = dto.DisplayOrder;
            faq.IsActive = dto.IsActive;
            faq.UpdatedAt = DateTime.UtcNow;

            faq.Translations.Clear();
            faq.Translations.Add(new FAQTranslation
            {
                Id = Guid.NewGuid(),
                FAQId = faq.Id,
                Language = Language.English,
                Question = dto.Question.English?.Trim() ?? string.Empty,
                Answer = dto.Answer.English?.Trim() ?? string.Empty
            });

            if (!string.IsNullOrWhiteSpace(dto.Question.German) || !string.IsNullOrWhiteSpace(dto.Answer.German))
            {
                faq.Translations.Add(new FAQTranslation
                {
                    Id = Guid.NewGuid(),
                    FAQId = faq.Id,
                    Language = Language.German,
                    Question = string.IsNullOrWhiteSpace(dto.Question.German) ? (dto.Question.English?.Trim() ?? string.Empty) : dto.Question.German.Trim(),
                    Answer = string.IsNullOrWhiteSpace(dto.Answer.German) ? (dto.Answer.English?.Trim() ?? string.Empty) : dto.Answer.German.Trim()
                });
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TripFAQUpdatedResponseDto(
                faq.Id,
                faq.TripId!.Value,
                faq.Question,
                faq.Answer,
                faq.DisplayOrder,
                faq.IsActive);
        }
    }
}