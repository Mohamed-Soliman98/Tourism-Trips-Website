using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Trips;
using Domain.Enum;

namespace Application.Services.Trips
{
    public class UnpublishTripService : IUnpublishTripService
    {
        private readonly IUnitOfWork _unitOfWork;

        public UnpublishTripService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TripUnpublishedResponseDto> UnpublishTripAsync(Guid tripId, CancellationToken cancellationToken)
        {
            var trip = await _unitOfWork.Trips.GetByIdAsync(tripId, cancellationToken);
            
            if (trip == null)
            {
                throw new KeyNotFoundException($"Trip with ID {tripId} not found.");
            }

            // Check if trip is already unpublished
            if (trip.Status == TripStatus.Draft)
            {
                throw new InvalidOperationException("Trip is already unpublished.");
            }

            // Unpublish the trip: Active → Draft
            trip.Status = TripStatus.Draft;
            trip.UpdatedAt = DateTime.UtcNow;

            _unitOfWork.Trips.Update(trip);
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