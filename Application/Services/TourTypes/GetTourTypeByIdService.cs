using Application.DTOs.TourTypes;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.TourTypes;

namespace Application.Services.TourTypes
{
    public class GetTourTypeByIdService : IGetTourTypeByIdService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetTourTypeByIdService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<TourTypeDetailDto> GetTourTypeByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Tour type Id cannot be empty.", nameof(id));
            }

            // Reuses Generic Repository: GetByIdAsync
            var tourType = await _unitOfWork.TourTypes.GetByIdAsync(id, cancellationToken);
            if (tourType == null)
            {
                throw new KeyNotFoundException($"Tour type with ID '{id}' was not found.");
            }

            return new TourTypeDetailDto(
                tourType.Id,
                tourType.Name,
                tourType.IsActive,
                tourType.CreatedAt,
                tourType.UpdatedAt
            );
        }
    }
}
