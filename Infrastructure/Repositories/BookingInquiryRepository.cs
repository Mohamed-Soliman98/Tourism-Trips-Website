using Application.DTOs.BookingInquiries;
using Application.Interfaces.Repositories;
using Domain.Entity;
using Domain.Enum;
using Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Infrastructure.Repositories
{
    public class BookingInquiryRepository : RepositoryGeneric<BookingInquiry>, IBookingInquiryRepository
    {
        public BookingInquiryRepository(AppDbContext context) : base(context)
        {
        }

        public async Task<(List<BookingInquiry> Items, int TotalCount)> GetInquiriesAsync(
            GetBookingInquiriesQueryDto query,
            CancellationToken cancellationToken = default)
        {
            var q = _dbSet.AsNoTracking();

            if (query.TripId.HasValue && query.TripId.Value != Guid.Empty)
            {
                q = q.Where(b => b.TripId == query.TripId.Value);
            }

            if (query.Status.HasValue)
            {
                q = q.Where(b => b.Status == query.Status.Value);
            }

            if (query.FromDate.HasValue)
            {
                q = q.Where(b => b.SelectedDate >= query.FromDate.Value.Date);
            }

            if (query.ToDate.HasValue)
            {
                q = q.Where(b => b.SelectedDate <= query.ToDate.Value.Date.AddDays(1).AddTicks(-1));
            }

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.Trim().ToLower();
                q = q.Where(b =>
                    b.Name.ToLower().Contains(search) ||
                    b.Email.ToLower().Contains(search) ||
                    b.Phone.ToLower().Contains(search) ||
                    (b.WhatsApp != null && b.WhatsApp.ToLower().Contains(search)) ||
                    b.Trip.Title.ToLower().Contains(search));
            }

            var totalCount = await q.CountAsync(cancellationToken);

            var items = await q
                .Include(b => b.Trip)
                .OrderByDescending(b => b.CreatedAt)
                .ThenByDescending(b => b.Id)
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<BookingInquiry?> GetByIdWithTripAsync(
            Guid id,
            CancellationToken cancellationToken = default)
        {
            return await _dbSet
                .AsNoTracking()
                .Include(b => b.Trip)
                .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
        }

        public async Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(cancellationToken);
        }

        public async Task<int> GetCountByStatusAsync(BookingInquiryStatus status, CancellationToken cancellationToken = default)
        {
            return await _dbSet.CountAsync(b => b.Status == status, cancellationToken);
        }
    }
}
