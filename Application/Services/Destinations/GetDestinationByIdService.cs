using Application.DTOs.Destinations;
using Application.Interfaces.Destinations;
using Application.Interfaces.IUnitOfWork;

namespace Application.Services.Destinations
{
    public class GetDestinationByIdService : IGetDestinationByIdService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetDestinationByIdService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<DestinationDetailDto> GetDestinationByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Destination Id cannot be empty.", nameof(id));
            }

            // Reuses Generic Repository: GetByIdAsync
            var destination = await _unitOfWork.Destinations.GetByIdAsync(id, cancellationToken);
            if (destination == null)
            {
                throw new KeyNotFoundException($"Destination with ID '{id}' was not found.");
            }

            return new DestinationDetailDto(
                destination.Id,
                destination.Name,
                destination.IsActive,
                destination.CreatedAt,
                destination.UpdatedAt
            );
        }
    }
}
