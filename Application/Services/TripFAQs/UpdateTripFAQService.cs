using Application.DTOs.TripFAQs;
using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.TripFAQs;
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

            faq.Question = dto.Question.Trim();
            faq.Answer = dto.Answer.Trim();
            faq.DisplayOrder = dto.DisplayOrder;
            faq.IsActive = dto.IsActive;
            faq.UpdatedAt = DateTime.UtcNow;

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