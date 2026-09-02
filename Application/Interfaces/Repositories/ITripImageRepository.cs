using Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ITripImageRepository : IRepositoryGeneric<TripImage>
    {
        /// <summary>
        /// Returns all images belonging to a trip, ordered by DisplayOrder ascending.
        /// </summary>
        Task<List<TripImage>> GetByTripIdAsync(
            Guid tripId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns true when the trip already has at least one image record.
        /// </summary>
        Task<bool> ExistsByTripIdAsync(
            Guid tripId,
            CancellationToken cancellationToken = default);
            
        /// <summary>
        /// Returns all tracked cover images for a trip (IsCover = true).
        /// Used for updating cover status in a tracked context.
        /// </summary>
        Task<List<TripImage>> GetCoverImagesTrackedAsync(
            Guid tripId,
            CancellationToken cancellationToken = default);
    }
}
