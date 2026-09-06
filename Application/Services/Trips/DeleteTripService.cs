using Application.DTOs.Trips;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.Storage;
using Application.Interfaces.Trips;

namespace Application.Services.Trips
{
    public class DeleteTripService : IDeleteTripService
    {
        private readonly ITripRepository _tripRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTripService(IUnitOfWork unitOfWork,ITripRepository tripRepository)
        {
            _unitOfWork = unitOfWork;
            _tripRepository = tripRepository;
           
        }

        public async Task<TripDeletedResponseDto> DeleteTripAsync(Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Trip Id cannot be empty.", nameof(id));
            }

            var trip = await _tripRepository.GetByIdForDeleteAsync(id, cancellationToken);
            if (trip == null)
            {
                throw new KeyNotFoundException($"Trip with ID '{id}' was not found.");
            }


            await using var transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);

            try
            {
                trip.IsDeleted = true;
                trip.DeletedAt = DateTime.UtcNow;
                trip.UpdatedAt = DateTime.UtcNow;

                _tripRepository.Update(trip);
                await _unitOfWork.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            }
            catch
            {
                await transaction.RollbackAsync(cancellationToken);
                throw;
            }

            

            return new TripDeletedResponseDto("Trip deleted successfully.");
        }
    }
}
