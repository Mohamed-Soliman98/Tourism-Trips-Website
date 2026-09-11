using Application.Interfaces.Auth;
using Application.Interfaces.CurrentUser;

namespace Application.Services.Auth
{
    public sealed class LogoutService : ILogoutService
    {
        private readonly ICurrentUserService _currentUserService;
        private readonly IRefreshTokenService _refreshTokenService;

        public LogoutService(
            ICurrentUserService currentUserService,
            IRefreshTokenService refreshTokenService)
        {
            _currentUserService = currentUserService;
            _refreshTokenService = refreshTokenService;
        }

        public async Task LogoutAsync(CancellationToken cancellationToken)
        {
            var userId = _currentUserService.UserId;

            if (userId == Guid.Empty)
            {
                throw new UnauthorizedAccessException("No authenticated user found.");
            }

            await _refreshTokenService.RevokeAllUserRefreshTokensAsync(userId, cancellationToken);
        }
    }
}
