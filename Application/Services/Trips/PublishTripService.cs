using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Trips;
using Domain.Enum;

namespace Application.Services.Trips
{
    public class PublishTripService : IPublishTripService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PublishTripService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TripPublishedResponseDto> PublishTripAsync(Guid tripId, CancellationToken cancellationToken)
        {
            var trip = await _unitOfWork.Trips.GetByIdAsync(tripId, cancellationToken);
            
            if (trip == null)
            {
                throw new KeyNotFoundException($"Trip with ID {tripId} not found.");
            }

            // Check if trip is already published
            if (trip.Status == TripStatus.Active)
            {
                throw new InvalidOperationException("Trip is already published.");
            }

            // Publish the trip: Draft → Active
            trip.Status = TripStatus.Active;
            trip.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Trips.Update(trip);
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