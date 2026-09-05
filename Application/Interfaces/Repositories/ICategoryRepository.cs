using Application.DTOs.Categories;
using Domain.Entity;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface ICategoryRepository : IRepositoryGeneric<Category>
    {
        Task<bool> ExistsAndIsActiveAsync( Guid id,CancellationToken cancellationToken);

        Task<bool> ExistsByNameAsync( string name, CancellationToken cancellationToken = default);

        Task<(List<Category> Items, int TotalCount)> GetCategoriesAsync( GetCategoriesQueryDto query,CancellationToken cancellationToken = default);

        Task<bool> ExistsByNameExcludingIdAsync( string name,Guid excludeId,CancellationToken cancellationToken = default);

        Task<List<Category>> GetActiveAsync(CancellationToken cancellationToken = default);
    }
}
