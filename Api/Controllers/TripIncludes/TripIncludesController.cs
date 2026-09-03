using Application.DTOs.TripIncludes;
using Application.DTOs.Trips;
using Application.Interfaces.TripIncludes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.TripIncludes
{
    [Route("api/trips/{tripId:guid}/includes")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class TripIncludesController : ControllerBase
    {
        private readonly IAddTripIncludeService _addTripIncludeService;
        private readonly IGetTripIncludesService _getTripIncludesService;
        private readonly IGetTripIncludeByIdService _getTripIncludeByIdService;
        private readonly IUpdateTripIncludeService _updateTripIncludeService;
        private readonly IDeleteTripIncludeService _deleteTripIncludeService;

        public TripIncludesController(
            IAddTripIncludeService addTripIncludeService,
            IGetTripIncludesService getTripIncludesService,
            IGetTripIncludeByIdService getTripIncludeByIdService,
            IUpdateTripIncludeService updateTripIncludeService,
            IDeleteTripIncludeService deleteTripIncludeService)
        {
            _addTripIncludeService = addTripIncludeService;
            _getTripIncludesService = getTripIncludesService;
            _getTripIncludeByIdService = getTripIncludeByIdService;
            _updateTripIncludeService = updateTripIncludeService;
            _deleteTripIncludeService = deleteTripIncludeService;
        }

        [HttpPost]
        public async Task<ActionResult<TripIncludeAddedResponseDto>> AddTripInclude(
            Guid tripId,
            [FromBody] CreateTripIncludeDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _addTripIncludeService.AddTripIncludeAsync(tripId, dto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpGet]
        public async Task<ActionResult<List<TripIncludeResponseDto>>> GetTripIncludes(
            Guid tripId,
            CancellationToken cancellationToken)
        {
            var result = await _getTripIncludesService.GetTripIncludesAsync(tripId, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{includeId:guid}")]
        public async Task<ActionResult<TripIncludeResponseDto>> GetTripIncludeById(
            Guid tripId,
            Guid includeId,
            CancellationToken cancellationToken)
        {
            var result = await _getTripIncludeByIdService.GetTripIncludeByIdAsync(
                tripId, includeId, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{includeId:guid}")]
        public async Task<ActionResult<TripIncludeUpdatedResponseDto>> UpdateTripInclude(
            Guid tripId,
            Guid includeId,
            [FromBody] UpdateTripIncludeDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _updateTripIncludeService.UpdateTripIncludeAsync(
                tripId, includeId, dto, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{includeId:guid}")]
        public async Task<ActionResult<TripIncludeDeletedResponseDto>> DeleteTripInclude(
            Guid tripId,
            Guid includeId,
            CancellationToken cancellationToken)
        {
            var result = await _deleteTripIncludeService.DeleteTripIncludeAsync(
                tripId, includeId, cancellationToken);
            return Ok(result);
        }
    }
}
