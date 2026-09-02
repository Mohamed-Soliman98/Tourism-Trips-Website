using Application.DTOs.TourTypes;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ITourTypeRepository : IRepositoryGeneric<TourType>
    {
        Task<bool> ExistsAndIsActiveAsync(
            Guid id,
            CancellationToken cancellationToken);

        Task<bool> ExistsByNameAsync(
            string name,
            CancellationToken cancellationToken = default);

        Task<(List<TourType> Items, int TotalCount)> GetTourTypesAsync(
            GetTourTypesQueryDto query,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsByNameExcludingIdAsync(
            string name,
            Guid excludeId,
            CancellationToken cancellationToken = default);

        Task<List<TourType>> GetActiveAsync(CancellationToken cancellationToken = default);

        Task<bool> HasTripsAsync(
            Guid tourTypeId,
            CancellationToken cancellationToken = default);
    }
}

