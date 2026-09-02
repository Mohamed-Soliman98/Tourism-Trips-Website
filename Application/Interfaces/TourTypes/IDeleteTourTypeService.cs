using Application.DTOs.TourTypes;

namespace Application.Interfaces.TourTypes
{
    public interface IDeleteTourTypeService
    {
        Task<TourTypeDeletedResponseDto> DeleteTourTypeAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}
