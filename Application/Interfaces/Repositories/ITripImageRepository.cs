using Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ITripImageRepository : IRepositoryGeneric<TripImage>
    {
        Task<List<TripImage>> GetByTripIdAsync(
            Guid tripId,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsByTripIdAsync(
            Guid tripId,
            CancellationToken cancellationToken = default);
            
        Task<List<TripImage>> GetCoverImagesTrackedAsync(
            Guid tripId,
            CancellationToken cancellationToken = default);
    }
}
