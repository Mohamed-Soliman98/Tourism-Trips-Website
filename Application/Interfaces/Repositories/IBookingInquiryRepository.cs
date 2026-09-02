using Application.DTOs.BookingInquiries;
using Domain.Entity;
using Domain.Enum;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces.Repositories
{
    public interface IBookingInquiryRepository : IRepositoryGeneric<BookingInquiry>
    {
        Task<(List<BookingInquiry> Items, int TotalCount)> GetInquiriesAsync(
            GetBookingInquiriesQueryDto query,
            CancellationToken cancellationToken = default);

        Task<BookingInquiry?> GetByIdWithTripAsync(
            Guid id,
            CancellationToken cancellationToken = default);

        Task<int> GetTotalCountAsync(CancellationToken cancellationToken = default);

        Task<int> GetCountByStatusAsync(BookingInquiryStatus status, CancellationToken cancellationToken = default);
    }
}
