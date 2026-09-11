using Application.DTOs.Auth;
using Application.Interfaces.Auth;
using Domain.Enum;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using UAParser;

namespace Infrastructure.Services.Auth;

public sealed class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        ITokenService tokenService,
        SignInManager<ApplicationUser> signInManager,
        IRefreshTokenService refreshTokenService,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _userManager = userManager;
        _tokenService = tokenService;
        _signInManager = signInManager;
        _refreshTokenService = refreshTokenService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<AuthResultDto> LoginAsync(LoginDto dto, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, lockoutOnFailure: true);

        if (result.IsLockedOut)
        {
            throw new UnauthorizedAccessException("Account temporarily locked due to multiple failed login attempts.");
        }

        if (!result.Succeeded)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var roles = await _userManager.GetRolesAsync(user);

        if (!roles.Contains(UserRole.SuperAdmin.ToString()) && !roles.Contains(UserRole.Admin.ToString()))
        {
            throw new UnauthorizedAccessException("User is not authorized as an admin.");
        }

        var (token, expiresOn) = _tokenService.GenerateToken(user.Id, user.Email!, roles);

        // Extract device/session info — all nullable, never breaks login
        var (deviceName, browser, operatingSystem, ipAddress) = ExtractClientInfo();

        var refreshToken = await _refreshTokenService.CreateRefreshTokenAsync(
            user.Id,
            deviceName,
            browser,
            operatingSystem,
            ipAddress,
            cancellationToken);

        return new AuthResultDto
        (
            Token: token,
            ExpiresAt: expiresOn,
            Email: user.Email!,
            Role: roles.First(),
            RefreshToken: refreshToken
        );
    }

   
    private (string? deviceName, string? browser, string? operatingSystem, string? ipAddress) ExtractClientInfo()
    {
        var httpContext = _httpContextAccessor.HttpContext;
        if (httpContext is null)
            return (null, null, null, null);

        // --- IP Address ---
        string? ipAddress = null;
        var forwardedFor = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();
        if (!string.IsNullOrWhiteSpace(forwardedFor))
        {
            // X-Forwarded-For may contain a comma-separated list; take the first (originating) IP
            ipAddress = forwardedFor.Split(',', StringSplitOptions.TrimEntries)[0];
        }
        else
        {
            ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();
        }

        // Truncate to column max length (nvarchar(45) — enough for IPv6)
        if (ipAddress is not null && ipAddress.Length > 45)
            ipAddress = ipAddress[..45];

        // --- User-Agent ---
        var userAgentHeader = httpContext.Request.Headers.UserAgent.ToString();
        if (string.IsNullOrWhiteSpace(userAgentHeader))
            return (null, null, null, ipAddress);

        // --- Parse User-Agent with UAParser ---
        string? browser = null;
        string? operatingSystem = null;
        string? deviceName = null;

        try
        {
            var parser = Parser.GetDefault();
            var clientInfo = parser.Parse(userAgentHeader);

            browser = clientInfo.UA.Family;                 // e.g. "Chrome", "Firefox"
            operatingSystem = clientInfo.OS.Family;         // e.g. "Windows", "iOS"
            deviceName = clientInfo.Device.Family;          // e.g. "iPhone", "Other"

            // Truncate to column max lengths defined in RefreshTokenConfiguration
            if (browser is not null && browser.Length > 100)
                browser = browser[..100];
            if (operatingSystem is not null && operatingSystem.Length > 100)
                operatingSystem = operatingSystem[..100];
            if (deviceName is not null && deviceName.Length > 200)
                deviceName = deviceName[..200];
        }
        catch
        {
            // Parsing must never prevent login — silently discard on any error
        }

        return (deviceName, browser, operatingSystem, ipAddress);
    }
}