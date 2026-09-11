namespace Application.DTOs.Auth
{
    /// <summary>Request body for POST /api/auth/refresh.</summary>
    public sealed record RefreshTokenRequestDto
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
