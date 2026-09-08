using Application.DTOs.TripHighlights;
using Application.DTOs.Trips;
using Application.Interfaces.TripHighlights;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.TripHighlights
{
    [Route("api/trips/{tripId:guid}/highlights")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class TripHighlightsController : ControllerBase
    {
        private readonly IAddTripHighlightService _addTripHighlightService;
        private readonly IGetTripHighlightsService _getTripHighlightsService;
        private readonly IGetTripHighlightByIdService _getTripHighlightByIdService;
        private readonly IUpdateTripHighlightService _updateTripHighlightService;
        private readonly IDeleteTripHighlightService _deleteTripHighlightService;

        public TripHighlightsController(
            IAddTripHighlightService addTripHighlightService,
            IGetTripHighlightsService getTripHighlightsService,
            IGetTripHighlightByIdService getTripHighlightByIdService,
            IUpdateTripHighlightService updateTripHighlightService,
            IDeleteTripHighlightService deleteTripHighlightService)
        {
            _addTripHighlightService = addTripHighlightService;
            _getTripHighlightsService = getTripHighlightsService;
            _getTripHighlightByIdService = getTripHighlightByIdService;
            _updateTripHighlightService = updateTripHighlightService;
            _deleteTripHighlightService = deleteTripHighlightService;
        }

        [HttpPost]
        public async Task<ActionResult<TripHighlightAddedResponseDto>> AddTripHighlight(
            Guid tripId,
            [FromBody] CreateTripHighlightDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _addTripHighlightService.AddTripHighlightAsync(tripId, dto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpGet]
        public async Task<ActionResult<List<TripHighlightResponseDto>>> GetTripHighlights(
            Guid tripId,
            CancellationToken cancellationToken)
        {
            var result = await _getTripHighlightsService.GetTripHighlightsAsync(tripId, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{highlightId:guid}")]
        public async Task<ActionResult<TripHighlightResponseDto>> GetTripHighlightById(
            Guid tripId,
            Guid highlightId,
            CancellationToken cancellationToken)
        {
            var result = await _getTripHighlightByIdService.GetTripHighlightByIdAsync(
                tripId, highlightId, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{highlightId:guid}")]
        public async Task<ActionResult<TripHighlightUpdatedResponseDto>> UpdateTripHighlight(
            Guid tripId,
            Guid highlightId,
            [FromBody] UpdateTripHighlightDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _updateTripHighlightService.UpdateTripHighlightAsync(
                tripId, highlightId, dto, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{highlightId:guid}")]
        public async Task<ActionResult<TripHighlightDeletedResponseDto>> DeleteTripHighlight(
            Guid tripId,
            Guid highlightId,
            CancellationToken cancellationToken)
        {
            var result = await _deleteTripHighlightService.DeleteTripHighlightAsync(
                tripId, highlightId, cancellationToken);
            return Ok(result);
        }
    }
}