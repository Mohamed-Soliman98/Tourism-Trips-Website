using Application.DTOs.SiteSettings;
using Application.Interfaces.SiteSettings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.SiteSettings
{
    [Route("api/site-settings")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class SiteSettingsController : ControllerBase
    {
        private readonly IGetSiteSettingsService _getSiteSettingsService;
        private readonly ICreateSiteSettingsService _createSiteSettingsService;
        private readonly IUpdateSiteSettingsService _updateSiteSettingsService;
        private readonly IGetPublicSiteSettingsService _getPublicSiteSettingsService;

        public SiteSettingsController(
            IGetSiteSettingsService getSiteSettingsService,
            ICreateSiteSettingsService createSiteSettingsService,
            IUpdateSiteSettingsService updateSiteSettingsService,
            IGetPublicSiteSettingsService getPublicSiteSettingsService)
        {
            _getSiteSettingsService = getSiteSettingsService;
            _createSiteSettingsService = createSiteSettingsService;
            _updateSiteSettingsService = updateSiteSettingsService;
            _getPublicSiteSettingsService = getPublicSiteSettingsService;
        }

        [HttpGet]
        public async Task<ActionResult<SiteSettingDto>> GetSiteSettings(CancellationToken cancellationToken)
        {
            var result = await _getSiteSettingsService.GetSiteSettingsAsync(cancellationToken);
            
            if (result == null)
            {
                return NotFound("Site settings not found. Use POST to create initial settings.");
            }

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<SiteSettingCreatedResponseDto>> CreateSiteSettings(
            [FromBody] CreateSiteSettingDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await _createSiteSettingsService.CreateSiteSettingsAsync(dto, cancellationToken);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<ActionResult<SiteSettingUpdatedResponseDto>> UpdateSiteSettings(
            [FromBody] UpdateSiteSettingDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _updateSiteSettingsService.UpdateSiteSettingsAsync(dto, cancellationToken);
            
            if (result == null)
            {
                return NotFound("Site settings not found. Use POST to create initial settings.");
            }

            return Ok(result);
        }

        /// <summary>
        /// Public – Get site settings for the website (public-safe fields only).
        /// </summary>
        [HttpGet("public")]
        [AllowAnonymous]
        public async Task<ActionResult<PublicSiteSettingDto>> GetPublicSiteSettings(CancellationToken cancellationToken)
        {
            var result = await _getPublicSiteSettingsService.GetPublicSiteSettingsAsync(cancellationToken);
            
            if (result == null)
            {
                return NotFound("Site settings not configured.");
            }

            return Ok(result);
        }
    }
}