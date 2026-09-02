using Application.DTOs.CMSSections;
using Application.DTOs.Common;
using Application.Interfaces.CMSSections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.CMSSections
{
    [Route("api/cms-sections")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CMSSectionsController : ControllerBase
    {
        private readonly ICreateCMSSectionService _createCMSSectionService;
        private readonly IGetCMSSectionsService _getCMSSectionsService;
        private readonly IGetCMSSectionByIdService _getCMSSectionByIdService;
        private readonly IUpdateCMSSectionService _updateCMSSectionService;
        private readonly IDeleteCMSSectionService _deleteCMSSectionService;
        private readonly IGetPublicCMSSectionsService _getPublicCMSSectionsService;

        public CMSSectionsController(
            ICreateCMSSectionService createCMSSectionService,
            IGetCMSSectionsService getCMSSectionsService,
            IGetCMSSectionByIdService getCMSSectionByIdService,
            IUpdateCMSSectionService updateCMSSectionService,
            IDeleteCMSSectionService deleteCMSSectionService,
            IGetPublicCMSSectionsService getPublicCMSSectionsService)
        {
            _createCMSSectionService = createCMSSectionService;
            _getCMSSectionsService = getCMSSectionsService;
            _getCMSSectionByIdService = getCMSSectionByIdService;
            _updateCMSSectionService = updateCMSSectionService;
            _deleteCMSSectionService = deleteCMSSectionService;
            _getPublicCMSSectionsService = getPublicCMSSectionsService;
        }

        [HttpPost]
        public async Task<ActionResult<CMSSectionCreatedResponseDto>> CreateCMSSection(
            [FromBody] CreateCMSSectionDto dto,
            CancellationToken cancellationToken)
        {
            try
            {
                var result = await _createCMSSectionService.CreateCMSSectionAsync(dto, cancellationToken);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<CMSSectionSummaryDto>>> GetCMSSections(
            [FromQuery] GetCMSSectionsQueryDto query,
            CancellationToken cancellationToken)
        {
            var result = await _getCMSSectionsService.GetCMSSectionsAsync(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CMSSectionDetailDto>> GetCMSSectionById(
            Guid id,
            CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid CMS section ID.");
            }

            var result = await _getCMSSectionByIdService.GetCMSSectionByIdAsync(id, cancellationToken);
            
            if (result == null)
            {
                return NotFound($"CMS section with ID {id} not found.");
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CMSSectionUpdatedResponseDto>> UpdateCMSSection(
            Guid id,
            [FromBody] UpdateCMSSectionDto dto,
            CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid CMS section ID.");
            }

            try
            {
                var result = await _updateCMSSectionService.UpdateCMSSectionAsync(id, dto, cancellationToken);
                
                if (result == null)
                {
                    return NotFound($"CMS section with ID {id} not found.");
                }

                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<CMSSectionDeletedResponseDto>> DeleteCMSSection(
            Guid id,
            CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid CMS section ID.");
            }

            var result = await _deleteCMSSectionService.DeleteCMSSectionAsync(id, cancellationToken);
            
            if (result == null)
            {
                return NotFound($"CMS section with ID {id} not found.");
            }

            return Ok(result);
        }

        /// <summary>
        /// Public – Get all active CMS sections for the website.
        /// </summary>
        [HttpGet("public")]
        [AllowAnonymous]
        public async Task<ActionResult<List<PublicCMSSectionDto>>> GetPublicCMSSections(CancellationToken cancellationToken)
        {
            var result = await _getPublicCMSSectionsService.GetPublicCMSSectionsAsync(cancellationToken);
            return Ok(result);
        }
    }
}