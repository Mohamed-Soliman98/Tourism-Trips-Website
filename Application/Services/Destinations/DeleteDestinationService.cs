using Application.DTOs.Destinations;
using Application.Interfaces.Destinations;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;

namespace Application.Services.Destinations
{
    public class DeleteDestinationService : IDeleteDestinationService
    {
        private readonly IDestinationRepository _destinationRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteDestinationService(
            IUnitOfWork unitOfWork,
            IDestinationRepository destinationRepository)
        {
            _unitOfWork = unitOfWork;
            _destinationRepository = destinationRepository;
        }

        public async Task<DestinationDeletedResponseDto> DeleteDestinationAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Destination Id cannot be empty.", nameof(id));
            }

            var destination = await _destinationRepository.GetByIdAsync(id, cancellationToken);
            if (destination == null)
            {
                throw new KeyNotFoundException($"Destination with ID '{id}' was not found.");
            }

            if (await _destinationRepository.HasTripsAsync(id, cancellationToken))
            {
                throw new InvalidOperationException(
                    $"Cannot delete destination '{destination.Name}' because it is associated with one or more trips.");
            }

            _destinationRepository.Remove(destination);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new DestinationDeletedResponseDto();
        }
    }
}
