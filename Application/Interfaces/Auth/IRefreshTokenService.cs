using Application.DTOs.Auth;

namespace Application.Interfaces.Auth
{
    public interface IRefreshTokenService
    {
       
        Task<string> CreateRefreshTokenAsync(
            Guid userId,
            string? deviceName,
            string? browser,
            string? operatingSystem,
            string? ipAddress,
            CancellationToken cancellationToken);
      
        Task<RefreshTokenDto> RefreshAsync(string refreshToken, CancellationToken cancellationToken);
       
        Task RevokeAllUserRefreshTokensAsync(Guid userId, CancellationToken cancellationToken);
    }
}
