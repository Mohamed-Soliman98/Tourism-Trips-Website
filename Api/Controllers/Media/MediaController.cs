using Application.DTOs.Media;
using Application.Interfaces.Media;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Media
{
    [Route("api/media")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class MediaController : ControllerBase
    {
        private readonly IUploadMediaService _uploadMediaService;

        public MediaController(IUploadMediaService uploadMediaService)
        {
            _uploadMediaService = uploadMediaService;
        }

        [HttpPost("upload")]
        [ProducesResponseType(typeof(MediaUploadedResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<MediaUploadedResponseDto>> Upload(
            [FromForm] UploadMediaDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _uploadMediaService.UploadAsync(dto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, result);
        }
    }
}
