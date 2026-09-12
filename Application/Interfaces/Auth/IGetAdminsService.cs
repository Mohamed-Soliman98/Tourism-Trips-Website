using Application.DTOs.Auth;
using Application.DTOs.Common;

namespace Application.Interfaces.Auth
{
    public interface IGetAdminsService
    {
        Task<PagedResult<AdminCreatedDto>> GetAdminsAsync(GetAdminsQueryDto query, CancellationToken cancellationToken = default);
    }
}
