using Application.DTOs.Common;
using Application.DTOs.Testimonials;
using Application.Interfaces.Repositories;
using Domain.Entity;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class TestimonialRepository : RepositoryGeneric<Testimonial>, ITestimonialRepository
    {
        public TestimonialRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<PagedResult<TestimonialSummaryDto>> GetTestimonialsAsync(GetTestimonialsQueryDto query, CancellationToken cancellationToken)
        {
            var queryable = _dbSet.AsQueryable();

            // Apply filters
            if (query.IsActive.HasValue)
            {
                queryable = queryable.Where(t => t.IsActive == query.IsActive.Value);
            }

            if (!string.IsNullOrEmpty(query.SearchTerm))
            {
                var searchTerm = query.SearchTerm.ToLower();
                queryable = queryable.Where(t => 
                    t.CustomerName.ToLower().Contains(searchTerm) ||
                    t.Content.ToLower().Contains(searchTerm) ||
                    (t.Country != null && t.Country.ToLower().Contains(searchTerm)));
            }

            // Get total count before pagination
            var totalCount = await queryable.CountAsync(cancellationToken);

            // Apply pagination and select
            var testimonials = await queryable
                .OrderByDescending(t => t.CreatedAt)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(t => new TestimonialSummaryDto(
                    t.Id,
                    t.CustomerName,
                    t.Country,
                    t.Rating,
                    t.Content,
                    t.IsActive,
                    t.CreatedAt
                ))
                .ToListAsync(cancellationToken);

            return new PagedResult<TestimonialSummaryDto>(
                testimonials,
                totalCount,
                query.Page,
                query.PageSize
            );
        }

        public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(cancellationToken);
        }

        public async Task<int> GetActiveCountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(t => t.IsActive, cancellationToken);
        }

        public async Task<int> GetInactiveCountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(t => !t.IsActive, cancellationToken);
        }
    }
}