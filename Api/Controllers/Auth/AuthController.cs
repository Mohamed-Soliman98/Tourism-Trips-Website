using Application.DTOs.Auth;
using Application.Interfaces.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Api.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAdminProfileService _adminProfileService;
        private readonly IChangePasswordService _changePasswordService;
        private readonly IRefreshTokenService _refreshTokenService;

        public AuthController(
            IAuthService authService,
            IAdminProfileService adminProfileService,
            IChangePasswordService changePasswordService,
            IRefreshTokenService refreshTokenService)
        {
            _authService = authService;
            _adminProfileService = adminProfileService;
            _changePasswordService = changePasswordService;
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost("login")]
        [EnableRateLimiting("LoginPolicy")]
        [ProducesResponseType(typeof(AuthResultDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(LoginDto dto, CancellationToken cancellationToken)
        {
            var result = await _authService.LoginAsync(dto, cancellationToken);

            return Ok(result);
        }

       
        [HttpPost("refresh")]
        [ProducesResponseType(typeof(RefreshTokenDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequestDto dto, CancellationToken cancellationToken)
        {
            var result = await _refreshTokenService.RefreshAsync(dto.RefreshToken, cancellationToken);

            return Ok(result);
        }

      
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet("profile")]
        [ProducesResponseType(typeof(AdminProfileDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult<AdminProfileDto>> GetProfile(CancellationToken cancellationToken)
        {
            var profile = await _adminProfileService.GetProfileAsync(cancellationToken);

            return Ok(profile);
        }

 
        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost("change-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto, CancellationToken cancellationToken)
        {
            await _changePasswordService.ChangePasswordAsync(dto, cancellationToken);

            return Ok(new
            {
                message = "Password changed successfully."
            });
        }
    }
}
