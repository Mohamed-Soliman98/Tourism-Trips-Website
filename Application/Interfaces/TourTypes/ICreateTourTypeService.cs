using Application.DTOs.TourTypes;

namespace Application.Interfaces.TourTypes
{
    public interface ICreateTourTypeService
    {
        Task<TourTypeCreatedResponseDto> CreateTourTypeAsync(
            CreateTourTypeDto dto,
            CancellationToken cancellationToken = default);
    }
}
