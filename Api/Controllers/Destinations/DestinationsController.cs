using Application.DTOs.Common;
using Application.DTOs.Destinations;
using Application.Interfaces.Destinations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Destinations
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class DestinationsController : ControllerBase
    {
        private readonly ICreateDestinationService _createDestinationService;
        private readonly IGetDestinationsService _getDestinationsService;
        private readonly IGetDestinationByIdService _getDestinationByIdService;
        private readonly IUpdateDestinationService _updateDestinationService;
        private readonly IDeleteDestinationService _deleteDestinationService;
        private readonly IGetPublicDestinationsService _getPublicDestinationsService;

        public DestinationsController(
            ICreateDestinationService createDestinationService,
            IGetDestinationsService getDestinationsService,
            IGetDestinationByIdService getDestinationByIdService,
            IUpdateDestinationService updateDestinationService,
            IDeleteDestinationService deleteDestinationService,
            IGetPublicDestinationsService getPublicDestinationsService)
        {
            _createDestinationService = createDestinationService;
            _getDestinationsService = getDestinationsService;
            _getDestinationByIdService = getDestinationByIdService;
            _updateDestinationService = updateDestinationService;
            _deleteDestinationService = deleteDestinationService;
            _getPublicDestinationsService = getPublicDestinationsService;
        }

        [HttpPost]
        public async Task<ActionResult<DestinationCreatedResponseDto>> CreateDestination(
            [FromBody] CreateDestinationDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _createDestinationService.CreateDestinationAsync(dto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<DestinationSummaryDto>>> GetDestinations(
            [FromQuery] GetDestinationsQueryDto query,
            CancellationToken cancellationToken)
        {
            var result = await _getDestinationsService.GetDestinationsAsync(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<DestinationDetailDto>> GetDestinationById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _getDestinationByIdService.GetDestinationByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<DestinationUpdatedResponseDto>> UpdateDestination(
            Guid id,
            [FromBody] UpdateDestinationDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _updateDestinationService.UpdateDestinationAsync(id, dto, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<DestinationDeletedResponseDto>> DeleteDestination(
            Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _deleteDestinationService.DeleteDestinationAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpGet("public")]
        [AllowAnonymous]
        public async Task<ActionResult<List<PublicDestinationDto>>> GetPublicDestinations(CancellationToken cancellationToken)
        {
            var result = await _getPublicDestinationsService.GetPublicDestinationsAsync(cancellationToken);
            return Ok(result);
        }
    }
}
