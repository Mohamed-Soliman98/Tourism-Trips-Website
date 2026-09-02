using Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IFAQRepository : IRepositoryGeneric<FAQ>
    {
        /// <summary>
        /// Returns all FAQs belonging to a trip, ordered by DisplayOrder ascending.
        /// </summary>
        Task<List<FAQ>> GetByTripIdAsync(
            Guid tripId,
            CancellationToken cancellationToken = default);

        /// <summary>
        /// Returns true when the trip already has at least one FAQ record.
        /// </summary>
        Task<bool> ExistsByTripIdAsync(
            Guid tripId,
            CancellationToken cancellationToken = default);
    }
}