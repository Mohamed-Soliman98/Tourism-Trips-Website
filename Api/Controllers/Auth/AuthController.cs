using Application.DTOs.Auth;
using Application.Interfaces.Auth;
using Infrastructure.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Auth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IAdminProfileService _adminProfileService;
        private readonly IChangePasswordService _changePasswordService;

        public AuthController(IAuthService authService,IAdminProfileService adminProfileService,IChangePasswordService changePasswordService)
        {
            _authService = authService;
            _adminProfileService = adminProfileService;
            _changePasswordService = changePasswordService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto,CancellationToken cancellationToken)
        {
            var result = await _authService.LoginAsync(dto, cancellationToken);

            return Ok(result);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpGet("profile")]
        public async Task<ActionResult<AdminProfileDto>> GetProfile(CancellationToken cancellationToken)
        {
            var profile = await _adminProfileService.GetProfileAsync(cancellationToken);

            return Ok(profile);
        }

        [Authorize(Roles = "SuperAdmin,Admin")]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePasswordDto dto,CancellationToken cancellationToken)
        {
            await _changePasswordService.ChangePasswordAsync(dto, cancellationToken);

            return Ok(new
            {
                message = "Password changed successfully."
            });
        }
    }
}
