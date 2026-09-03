using Application.DTOs.TourTypes;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.TourTypes;

namespace Application.Services.TourTypes
{
    public class DeleteTourTypeService : IDeleteTourTypeService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTourTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TourTypeDeletedResponseDto> DeleteTourTypeAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Tour type Id cannot be empty.", nameof(id));
            }

            var tourType = await _unitOfWork.TourTypes.GetByIdAsync(id, cancellationToken);
            if (tourType == null)
            {
                throw new KeyNotFoundException($"Tour type with ID '{id}' was not found.");
            }

            if (await _unitOfWork.TourTypes.HasTripsAsync(id, cancellationToken))
            {
                throw new InvalidOperationException("Cannot delete tour type because it is associated with one or more trips.");
            }

            _unitOfWork.TourTypes.Remove(tourType);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TourTypeDeletedResponseDto();
        }
    }
}
