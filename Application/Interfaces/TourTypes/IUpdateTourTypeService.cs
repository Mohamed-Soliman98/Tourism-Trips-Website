using Application.DTOs.TourTypes;

namespace Application.Interfaces.TourTypes
{
    public interface IUpdateTourTypeService
    {
        Task<TourTypeUpdatedResponseDto> UpdateTourTypeAsync(
            Guid id,
            UpdateTourTypeDto dto,
            CancellationToken cancellationToken = default);
    }
}
