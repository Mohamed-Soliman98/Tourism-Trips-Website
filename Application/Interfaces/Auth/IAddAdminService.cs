using Application.DTOs.Auth;

namespace Application.Interfaces.Auth
{
    public interface IAddAdminService
    {
        Task<AdminCreatedDto> AddAdminAsync(CreateAdminRequestDto dto, CancellationToken cancellationToken);
    }
}
