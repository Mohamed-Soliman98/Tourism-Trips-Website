using Application.DTOs.Auth;
using Application.Interfaces.Auth;
using Domain.Enum;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Services.Auth;

public sealed class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ITokenService _tokenService;

    public AuthService(UserManager<ApplicationUser> userManager,ITokenService tokenService)
    {
        _userManager = userManager;
        _tokenService = tokenService;
    }

    public async Task<AuthResultDto> LoginAsync(LoginDto dto,CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null)
        {
            throw new UnauthorizedAccessException("Invalid email or password.");
        }

        var passwordValid = await _userManager.CheckPasswordAsync(user,dto.Password);

        if (!passwordValid)
        {
            throw new UnauthorizedAccessException( "Invalid email or password.");
        }

        var roles = await _userManager.GetRolesAsync(user);

        if (!roles.Contains(UserRole.SuperAdmin.ToString()) &&!roles.Contains(UserRole.Admin.ToString()))
        {
            throw new UnauthorizedAccessException("User is not authorized as an admin.");
        }

        var (token, expiresOn) = _tokenService.GenerateToken(user.Id,user.Email!,roles);

        return new AuthResultDto
        (
          Token: token,
          ExpiresAt: expiresOn,
          Email: user.Email!,
          Role: roles.First()
        );
    }
}