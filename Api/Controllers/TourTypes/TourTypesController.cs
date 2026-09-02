using Application.DTOs.Common;
using Application.DTOs.TourTypes;
using Application.Interfaces.TourTypes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.TourTypes
{
    [Route("api/tour-types")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class TourTypesController : ControllerBase
    {
        private readonly ICreateTourTypeService _createTourTypeService;
        private readonly IGetTourTypesService _getTourTypesService;
        private readonly IGetTourTypeByIdService _getTourTypeByIdService;
        private readonly IUpdateTourTypeService _updateTourTypeService;
        private readonly IDeleteTourTypeService _deleteTourTypeService;
        private readonly IGetPublicTourTypesService _getPublicTourTypesService;

        public TourTypesController(
            ICreateTourTypeService createTourTypeService,
            IGetTourTypesService getTourTypesService,
            IGetTourTypeByIdService getTourTypeByIdService,
            IUpdateTourTypeService updateTourTypeService,
            IDeleteTourTypeService deleteTourTypeService,
            IGetPublicTourTypesService getPublicTourTypesService)
        {
            _createTourTypeService = createTourTypeService;
            _getTourTypesService = getTourTypesService;
            _getTourTypeByIdService = getTourTypeByIdService;
            _updateTourTypeService = updateTourTypeService;
            _deleteTourTypeService = deleteTourTypeService;
            _getPublicTourTypesService = getPublicTourTypesService;
        }

        [HttpPost]
        public async Task<ActionResult<TourTypeCreatedResponseDto>> CreateTourType(
            [FromBody] CreateTourTypeDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _createTourTypeService.CreateTourTypeAsync(dto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<TourTypeSummaryDto>>> GetTourTypes(
            [FromQuery] GetTourTypesQueryDto query,
            CancellationToken cancellationToken)
        {
            var result = await _getTourTypesService.GetTourTypesAsync(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TourTypeDetailDto>> GetTourTypeById(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _getTourTypeByIdService.GetTourTypeByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<TourTypeUpdatedResponseDto>> UpdateTourType(
            [FromRoute] Guid id,
            [FromBody] UpdateTourTypeDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _updateTourTypeService.UpdateTourTypeAsync(id, dto, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<TourTypeDeletedResponseDto>> DeleteTourType(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _deleteTourTypeService.DeleteTourTypeAsync(id, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Public – Get all active tour types for website navigation and filters.
        /// </summary>
        [HttpGet("public")]
        [AllowAnonymous]
        public async Task<ActionResult<List<PublicTourTypeDto>>> GetPublicTourTypes(CancellationToken cancellationToken)
        {
            var result = await _getPublicTourTypesService.GetPublicTourTypesAsync(cancellationToken);
            return Ok(result);
        }
    }
}
