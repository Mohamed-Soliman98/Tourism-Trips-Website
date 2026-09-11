using Application.DTOs.Auth;
using Application.Interfaces.Auth;
using Application.Interfaces.IUnitOfWork;
using Domain.Entitys;
using Infrastructure.Identity;
using Infrastructure.Persistence;
using Infrastructure.Services.UnitOfWork;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Cryptography;

namespace Infrastructure.Services.Auth;

public sealed class RefreshTokenService : IRefreshTokenService
{
    private readonly AppDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtOptions _jwtOptions;

    public RefreshTokenService
         (AppDbContext context,
        UserManager<ApplicationUser> userManager,
        ITokenService tokenService,
        IOptions<JwtOptions> jwtOptions,
        IUnitOfWork unitOfWork
        )
    {
        _context = context;
        _userManager = userManager;
        _tokenService = tokenService;
        _jwtOptions = jwtOptions.Value;
        _unitOfWork = unitOfWork;

    }

    public async Task<string> CreateRefreshTokenAsync(
        Guid userId,
        string? deviceName,
        string? browser,
        string? operatingSystem,
        string? ipAddress,
        CancellationToken cancellationToken)
    {
        var tokenBytes = RandomNumberGenerator.GetBytes(64);
        var tokenValue = Convert.ToBase64String(tokenBytes);

        var refreshToken = new RefreshToken
        {
            UserId = userId,
            Token = tokenValue,
            DeviceName = deviceName,
            Browser = browser,
            OperatingSystem = operatingSystem,
            IpAddress = ipAddress,
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpiryDays)
        };

        _context.RefreshTokens.Add(refreshToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return tokenValue;
    }

    public async Task<RefreshTokenDto> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
    {
   
        const string invalidTokenMessage = "Invalid or expired refresh token.";

        var storedToken = await _context.RefreshTokens
            .FirstOrDefaultAsync(rt => rt.Token == refreshToken, cancellationToken);

        if (storedToken is null || !storedToken.IsActive)
        {
            throw new UnauthorizedAccessException(invalidTokenMessage);
        }

        var user = await _userManager.FindByIdAsync(storedToken.UserId.ToString());

        if (user is null)
        {
            throw new UnauthorizedAccessException(invalidTokenMessage);
        }

        if (await _userManager.IsLockedOutAsync(user))
        {
            throw new UnauthorizedAccessException(invalidTokenMessage);
        }

        storedToken.RevokedAt = DateTime.UtcNow;

        var newTokenBytes = RandomNumberGenerator.GetBytes(64);
        var newTokenValue = Convert.ToBase64String(newTokenBytes);

        var newRefreshToken = new RefreshToken
        {
            UserId = user.Id,
            Token = newTokenValue,

            DeviceName = storedToken.DeviceName,
            Browser = storedToken.Browser,
            OperatingSystem = storedToken.OperatingSystem,
            IpAddress = storedToken.IpAddress,

            CreatedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow.AddDays(_jwtOptions.RefreshTokenExpiryDays)
        };

        _context.RefreshTokens.Add(newRefreshToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var roles = await _userManager.GetRolesAsync(user);
        var (accessToken, _) = _tokenService.GenerateToken(user.Id, user.Email!, roles);

        return new RefreshTokenDto(AccessToken: accessToken, RefreshToken: newTokenValue);
    }

    public async Task RevokeAllUserRefreshTokensAsync(Guid userId, CancellationToken cancellationToken)
    {
        var activeTokens = await _context.RefreshTokens
            .Where(rt => rt.UserId == userId && rt.RevokedAt == null && rt.ExpiresAt > DateTime.UtcNow)
            .ToListAsync(cancellationToken);

        foreach (var token in activeTokens)
        {
            token.RevokedAt = DateTime.UtcNow;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
