using Application.DTOs.TourTypes;
using Application.Interfaces.Repositories;
using Application.Interfaces.TourTypes;

namespace Application.Services.TourTypes
{
    public class GetTourTypeByIdService : IGetTourTypeByIdService
    {
        private readonly ITourTypeRepository _tourTypeRepository;

        public GetTourTypeByIdService(ITourTypeRepository tourTypeRepository)
        {
            _tourTypeRepository = tourTypeRepository;
        }

        public async Task<TourTypeDetailDto> GetTourTypeByIdAsync(
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
