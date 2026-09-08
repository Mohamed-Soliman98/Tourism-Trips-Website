using Application.DTOs.Trips;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.Trips
{
    public interface ICreateTripService
    {
        Task<TripCreatedResponseDto> CreateTripAsync(CreateTripDto dto, CancellationToken cancellationToken = default);
    }
}
