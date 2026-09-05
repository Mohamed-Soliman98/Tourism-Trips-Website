using Application.DTOs.TourTypes;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Application.Interfaces.TourTypes;

namespace Application.Services.TourTypes
{
    public class DeleteTourTypeService : IDeleteTourTypeService
    {
        private readonly ITourTypeRepository _tourTypeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteTourTypeService(
            IUnitOfWork unitOfWork,
            ITourTypeRepository tourTypeRepository)
        {
            _unitOfWork = unitOfWork;
            _tourTypeRepository = tourTypeRepository;
        }

        public async Task<TourTypeDeletedResponseDto> DeleteTourTypeAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Tour type Id cannot be empty.", nameof(id));
            }

            var tourType = await _tourTypeRepository.GetByIdAsync(id, cancellationToken);
            if (tourType == null)
            {
                throw new KeyNotFoundException($"Tour type with ID '{id}' was not found.");
            }

            if (await _tourTypeRepository.HasTripsAsync(id, cancellationToken))
            {
                throw new InvalidOperationException("Cannot delete tour type because it is associated with one or more trips.");
            }

            _tourTypeRepository.Remove(tourType);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new TourTypeDeletedResponseDto();
        }
    }
}
