using Application.DTOs.Banners;
using Application.DTOs.Common;
using Application.Interfaces.Banners;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Banners
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class BannersController : ControllerBase
    {
        private readonly ICreateBannerService _createBannerService;
        private readonly IGetBannersService _getBannersService;
        private readonly IGetBannerByIdService _getBannerByIdService;
        private readonly IUpdateBannerService _updateBannerService;
        private readonly IDeleteBannerService _deleteBannerService;
        private readonly IGetPublicBannersService _getPublicBannersService;

        public BannersController(
            ICreateBannerService createBannerService,
            IGetBannersService getBannersService,
            IGetBannerByIdService getBannerByIdService,
            IUpdateBannerService updateBannerService,
            IDeleteBannerService deleteBannerService,
            IGetPublicBannersService getPublicBannersService)
        {
            _createBannerService = createBannerService;
            _getBannersService = getBannersService;
            _getBannerByIdService = getBannerByIdService;
            _updateBannerService = updateBannerService;
            _deleteBannerService = deleteBannerService;
            _getPublicBannersService = getPublicBannersService;
        }

        [HttpPost]
        public async Task<ActionResult<BannerCreatedResponseDto>> CreateBanner(
            [FromBody] CreateBannerDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _createBannerService.CreateBannerAsync(dto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<BannerSummaryDto>>> GetBanners(
            [FromQuery] GetBannersQueryDto query,
            CancellationToken cancellationToken)
        {
            var result = await _getBannersService.GetBannersAsync(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BannerDetailDto>> GetBannerById(
            Guid id,
            CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid banner ID.");
            }

            var result = await _getBannerByIdService.GetBannerByIdAsync(id, cancellationToken);
            
            if (result == null)
            {
                return NotFound($"Banner with ID {id} not found.");
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BannerUpdatedResponseDto>> UpdateBanner(
            Guid id,
            [FromBody] UpdateBannerDto dto,
            CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid banner ID.");
            }

            var result = await _updateBannerService.UpdateBannerAsync(id, dto, cancellationToken);
            
            if (result == null)
            {
                return NotFound($"Banner with ID {id} not found.");
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<BannerDeletedResponseDto>> DeleteBanner(
            Guid id,
            CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid banner ID.");
            }

            var result = await _deleteBannerService.DeleteBannerAsync(id, cancellationToken);
            
            if (result == null)
            {
                return NotFound($"Banner with ID {id} not found.");
            }

            return Ok(result);
        }

        [HttpGet("public")]
        [AllowAnonymous]
        public async Task<ActionResult<List<PublicBannerDto>>> GetPublicBanners(CancellationToken cancellationToken)
        {
            var result = await _getPublicBannersService.GetPublicBannersAsync(cancellationToken);
            return Ok(result);
        }
    }
}