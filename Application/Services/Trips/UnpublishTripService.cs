using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.Trips;
using Domain.Enum;

namespace Application.Services.Trips
{
    public class UnpublishTripService : IUnpublishTripService
    {
        private readonly ITripRepository _tripRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UnpublishTripService(
            IUnitOfWork unitOfWork,
            ITripRepository tripRepository)
        {
            _unitOfWork = unitOfWork;
            _tripRepository = tripRepository;
        }

        public async Task<TripUnpublishedResponseDto> UnpublishTripAsync(Guid tripId, CancellationToken cancellationToken)
        {
            var trip = await _tripRepository.GetByIdAsync(tripId, cancellationToken);
            
            if (trip == null)
            {
                throw new KeyNotFoundException($"Trip with ID {tripId} not found.");
            }

            if (trip.Status == TripStatus.Draft)
            {
                throw new InvalidOperationException("Trip is already unpublished.");
            }

            trip.Status = TripStatus.Draft;
            trip.UpdatedAt = DateTime.UtcNow;

            _tripRepository.Update(trip);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TripUnpublishedResponseDto(
                trip.Id,
                trip.Title,
                trip.Status,
                trip.UpdatedAt.Value
            );
        }
    }
}