using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.Trips;
using Domain.Enum;

namespace Application.Services.Trips
{
    public class PublishTripService : IPublishTripService
    {
        private readonly ITripRepository _tripRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PublishTripService(
            IUnitOfWork unitOfWork,
            ITripRepository tripRepository)
        {
            _unitOfWork = unitOfWork;
            _tripRepository = tripRepository;
        }

        public async Task<TripPublishedResponseDto> PublishTripAsync(Guid tripId, CancellationToken cancellationToken)
        {
            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            
            if (trip == null)
            {
                throw new KeyNotFoundException($"Trip with ID {tripId} not found.");
            }

            if (trip.Status == TripStatus.Active)
            {
                throw new InvalidOperationException("Trip is already published.");
            }

            trip.Status = TripStatus.Active;
            trip.UpdatedAt = DateTime.UtcNow;

            _tripRepository.Update(trip);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TripPublishedResponseDto(
                trip.Id,
                trip.Title,
                trip.Status,
                trip.UpdatedAt.Value
            );
        }
    }
}