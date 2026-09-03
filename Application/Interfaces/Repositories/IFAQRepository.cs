using Domain.Entitys;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IFAQRepository : IRepositoryGeneric<FAQ>
    {
        Task<List<FAQ>> GetByTripIdAsync(
            Guid tripId,
            CancellationToken cancellationToken = default);

        Task<bool> ExistsByTripIdAsync(
            Guid tripId,
            CancellationToken cancellationToken = default);
    }
}