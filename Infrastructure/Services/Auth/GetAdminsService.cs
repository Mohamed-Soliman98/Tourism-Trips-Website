using Application.DTOs.Auth;
using Application.DTOs.Common;
using Application.Interfaces.Auth;
using Domain.Enum;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Services.Auth
{
    public sealed class GetAdminsService : IGetAdminsService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly AppDbContext _dbContext;

        public GetAdminsService(UserManager<ApplicationUser> userManager, AppDbContext dbContext)
        {
            _userManager = userManager;
            _dbContext = dbContext;
        }

        public async Task<PagedResult<AdminCreatedDto>> GetAdminsAsync(
            GetAdminsQueryDto query,
            CancellationToken cancellationToken = default)
        {
            var adminRoleName = UserRole.Admin.ToString();

            // Resolve the Admin role ID at database level
            var adminRoleId = await _dbContext.Roles
                .Where(r => r.Name == adminRoleName)
                .Select(r => r.Id)
                .FirstOrDefaultAsync(cancellationToken);

            if (adminRoleId == default)
            {
                // Role has not been seeded yet — return an empty paginated result
                return new PagedResult<AdminCreatedDto>([], query.Page, query.PageSize, 0);
            }

            // Database-level filter: users who have the Admin role via the UserRoles join table
            var baseQuery = _dbContext.Users
                .Where(u => _dbContext.UserRoles
                    .Any(ur => ur.UserId == u.Id && ur.RoleId == adminRoleId))
                .OrderBy(u => u.FullName)
                .ThenBy(u => u.Email);

            var totalCount = await baseQuery.CountAsync(cancellationToken);

            var items = await baseQuery
                .Skip((query.Page - 1) * query.PageSize)
                .Take(query.PageSize)
                .Select(u => new AdminCreatedDto(
                    u.Id,
                    u.FullName,
                    u.Email!,
                    adminRoleName))
                .ToListAsync(cancellationToken);

            return new PagedResult<AdminCreatedDto>(items, query.Page, query.PageSize, totalCount);
        }
    }
}
