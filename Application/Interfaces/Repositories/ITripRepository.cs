using Application.DTOs.Trips;
using Domain.Entity;
using Domain.Enum;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ITripRepository : IRepositoryGeneric<Trip>
    {
        Task<bool> ExistsBySlugAsync(
            string slug,
            CancellationToken cancellationToken);

        Task<Trip?> GetByIdWithDetailsAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<Trip?> GetBySlugWithDetailsAsync(
            string slug,
            CancellationToken cancellationToken = default);

        Task<(List<Trip> Items, int TotalCount)> GetPublicTripsAsync(
            GetPublicTripsQueryDto query,
            CancellationToken cancellationToken = default);

        Task<(List<Trip> Items, int TotalCount)> GetAdminTripsAsync(
            GetAdminTripsQueryDto query,
            CancellationToken cancellationToken = default);

        Task<Trip?> GetByIdForUpdateAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsBySlugOtherThanIdAsync(
            string slug,
            Guid id,
            CancellationToken cancellationToken = default);

        Task<Trip?> GetByIdForDeleteAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);

        Task<int> GetCountByStatusAsync(TripStatus status, CancellationToken cancellationToken = default);
    }
}
