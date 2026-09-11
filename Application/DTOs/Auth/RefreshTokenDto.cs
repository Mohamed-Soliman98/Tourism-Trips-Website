namespace Application.DTOs.Auth
{
    /// <summary>Response returned by POST /api/auth/refresh.</summary>
    public sealed record RefreshTokenDto
    (
        string AccessToken,
        string RefreshToken
    );
}
