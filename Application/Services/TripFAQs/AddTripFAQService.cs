using Application.DTOs.TripFAQs;
using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripFAQs;
using Domain.Entitys;
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
                Question = dto.Question.Trim(),
                Answer = dto.Answer.Trim(),
                DisplayOrder = dto.DisplayOrder,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

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