using Application.DTOs.TourTypes;

namespace Application.Interfaces.TourTypes
{
    public interface IGetTourTypeByIdService
    {
        Task<TourTypeDetailDto> GetTourTypeByIdAsync(
            Guid id,
            CancellationToken cancellationToken = default);
    }
}
