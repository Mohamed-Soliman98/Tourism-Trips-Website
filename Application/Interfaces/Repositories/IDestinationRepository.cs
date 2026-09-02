using Application.DTOs.Destinations;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IDestinationRepository : IRepositoryGeneric<Destination>
    {
        Task<bool> ExistsAndIsActiveAsync(
            Guid id,
            CancellationToken cancellationToken);

        Task<bool> ExistsByNameAsync(
            string name,
            CancellationToken cancellationToken = default);

        Task<(List<Destination> Items, int TotalCount)> GetDestinationsAsync(
            GetDestinationsQueryDto query,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsByNameExcludingIdAsync(
            string name,
            Guid excludeId,
            CancellationToken cancellationToken = default);

        Task<List<Destination>> GetActiveAsync(CancellationToken cancellationToken = default);

        Task<bool> HasTripsAsync(
            Guid destinationId,
            CancellationToken cancellationToken = default);
    }
}
