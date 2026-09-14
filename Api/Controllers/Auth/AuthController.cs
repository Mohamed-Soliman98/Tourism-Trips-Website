using Application.DTOs.Auth;
using Application.DTOs.Common;
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
        private readonly ILogoutService _logoutService;
        private readonly IAddAdminService _addAdminService;
        private readonly IGetAdminsService _getAdminsService;

        public AuthController(
            IAuthService authService,
            IAdminProfileService adminProfileService,
            IChangePasswordService changePasswordService,
            IRefreshTokenService refreshTokenService,
            ILogoutService logoutService,
            IAddAdminService addAdminService,
            IGetAdminsService getAdminsService)
        {
            _authService = authService;
            _adminProfileService = adminProfileService;
            _changePasswordService = changePasswordService;
            _refreshTokenService = refreshTokenService;
            _logoutService = logoutService;
            _addAdminService = addAdminService;
            _getAdminsService = getAdminsService;
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


        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Logout(CancellationToken cancellationToken)
        {
            await _logoutService.LogoutAsync(cancellationToken);

            return NoContent();
        }

        [Authorize(Roles = "SuperAdmin")]
        [HttpPost("admins")]
        [ProducesResponseType(typeof(AdminCreatedDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddAdmin(CreateAdminRequestDto dto, CancellationToken cancellationToken)
        {
            var result = await _addAdminService.AddAdminAsync(dto, cancellationToken);

            return StatusCode(StatusCodes.Status201Created, result);
        }


        [Authorize(Roles = "SuperAdmin")]
        [HttpGet("all-admins")]
        [ProducesResponseType(typeof(PagedResult<AdminCreatedDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PagedResult<AdminListItemDto>>> GetAdmins( [FromQuery] GetAdminsQueryDto query, CancellationToken cancellationToken)
        {
            var result = await _getAdminsService.GetAdminsAsync(query, cancellationToken);

            return Ok(result);
        }
    }
}
