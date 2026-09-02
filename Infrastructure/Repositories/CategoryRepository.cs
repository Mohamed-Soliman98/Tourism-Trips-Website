using Application.DTOs.Categories;
using Application.Interfaces.Repositories;
using Domain.Entity;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : RepositoryGeneric<Category>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> ExistsAndIsActiveAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            return await _dbSet.AnyAsync(x => x.Id == id && x.IsActive, cancellationToken);
        }

        public async Task<bool> ExistsByNameAsync(
            string name,
            CancellationToken cancellationToken = default)
        {
            var normalizedName = name.Trim().ToLower();
            return await _dbSet.AnyAsync(x => x.Name.ToLower() == normalizedName, cancellationToken);
        }

        public async Task<(List<Category> Items, int TotalCount)> GetCategoriesAsync(
            GetCategoriesQueryDto query,
            CancellationToken cancellationToken = default)
        {
            var q = _dbSet.AsNoTracking();

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.Trim().ToLower();
                q = q.Where(c => c.Name.ToLower().Contains(search));
            }

            if (query.IsActive.HasValue)
            {
                q = q.Where(c => c.IsActive == query.IsActive.Value);
            }

            var totalCount = await q.CountAsync(cancellationToken);

            var items = await q
                .OrderBy(c => c.Name)
                .ThenBy(c => c.Id)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<bool> ExistsByNameExcludingIdAsync(
            string name,
            Guid excludeId,
            CancellationToken cancellationToken = default)
        {
            var normalizedName = name.Trim().ToLower();
            return await _dbSet.AnyAsync(
                c => c.Name.ToLower() == normalizedName && c.Id != excludeId,
                cancellationToken);
        }

        public async Task<List<Category>> GetActiveAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Where(c => c.IsActive)
                .OrderBy(c => c.Name)
                .ThenBy(c => c.Id)
                .ToListAsync(cancellationToken);
        }
    }
}

