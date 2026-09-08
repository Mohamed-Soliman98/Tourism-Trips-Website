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
    public class AddTripFAQService : IAddTripFAQService
    {
        private readonly ITripRepository _tripRepository;
        private readonly IFAQRepository _faqRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateTripFAQDto> _validator;

        public AddTripFAQService(
            IUnitOfWork unitOfWork,
            ITripRepository tripRepository,
            IFAQRepository faqRepository,
            IValidator<CreateTripFAQDto> validator)
        {
            _unitOfWork = unitOfWork;
            _tripRepository = tripRepository;
            _faqRepository = faqRepository;
            _validator = validator;
        }

        public async Task<TripFAQAddedResponseDto> AddTripFAQAsync(
            Guid tripId,
            CreateTripFAQDto dto,
            CancellationToken cancellationToken = default)
        {
            if (tripId == Guid.Empty)
                throw new ArgumentException("Trip Id cannot be empty.", nameof(tripId));

            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            if (trip == null)
                throw new KeyNotFoundException($"Trip with ID '{tripId}' was not found.");

            var faq = new FAQ
            {
                Id = Guid.NewGuid(),
                TripId = tripId,
                Question = dto.Question.English?.Trim() ?? string.Empty,
                Answer = dto.Answer.English?.Trim() ?? string.Empty,
                DisplayOrder = dto.DisplayOrder,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

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

            _faqRepository.Add(faq);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TripFAQAddedResponseDto(
                faq.Id,
                faq.TripId!.Value,
                faq.Question,
                faq.Answer,
                faq.DisplayOrder,
                faq.IsActive);
        }
    }
}