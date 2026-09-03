using Application.DTOs.TripExcludes;
using Application.DTOs.Trips;
using Application.Interfaces.TripExcludes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.TripExcludes
{
    [Route("api/trips/{tripId:guid}/excludes")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class TripExcludesController : ControllerBase
    {
        private readonly IAddTripExcludeService _addTripExcludeService;
        private readonly IGetTripExcludesService _getTripExcludesService;
        private readonly IGetTripExcludeByIdService _getTripExcludeByIdService;
        private readonly IUpdateTripExcludeService _updateTripExcludeService;
        private readonly IDeleteTripExcludeService _deleteTripExcludeService;

        public TripExcludesController(
            IAddTripExcludeService addTripExcludeService,
            IGetTripExcludesService getTripExcludesService,
            IGetTripExcludeByIdService getTripExcludeByIdService,
            IUpdateTripExcludeService updateTripExcludeService,
            IDeleteTripExcludeService deleteTripExcludeService)
        {
            _addTripExcludeService = addTripExcludeService;
            _getTripExcludesService = getTripExcludesService;
            _getTripExcludeByIdService = getTripExcludeByIdService;
            _updateTripExcludeService = updateTripExcludeService;
            _deleteTripExcludeService = deleteTripExcludeService;
        }

        [HttpPost]
        public async Task<ActionResult<TripExcludeAddedResponseDto>> AddTripExclude(
            Guid tripId,
            [FromBody] CreateTripExcludeDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _addTripExcludeService.AddTripExcludeAsync(tripId, dto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpGet]
        public async Task<ActionResult<List<TripExcludeResponseDto>>> GetTripExcludes(
            Guid tripId,
            CancellationToken cancellationToken)
        {
            var result = await _getTripExcludesService.GetTripExcludesAsync(tripId, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{excludeId:guid}")]
        public async Task<ActionResult<TripExcludeResponseDto>> GetTripExcludeById(
            Guid tripId,
            Guid excludeId,
            CancellationToken cancellationToken)
        {
            var result = await _getTripExcludeByIdService.GetTripExcludeByIdAsync(
                tripId, excludeId, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{excludeId:guid}")]
        public async Task<ActionResult<TripExcludeUpdatedResponseDto>> UpdateTripExclude(
            Guid tripId,
            Guid excludeId,
            [FromBody] UpdateTripExcludeDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _updateTripExcludeService.UpdateTripExcludeAsync(
                tripId, excludeId, dto, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{excludeId:guid}")]
        public async Task<ActionResult<TripExcludeDeletedResponseDto>> DeleteTripExclude(
            Guid tripId,
            Guid excludeId,
            CancellationToken cancellationToken)
        {
            var result = await _deleteTripExcludeService.DeleteTripExcludeAsync(
                tripId, excludeId, cancellationToken);
            return Ok(result);
        }
    }
}
