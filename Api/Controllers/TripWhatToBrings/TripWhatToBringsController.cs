using Application.DTOs.TripWhatToBrings;
using Application.DTOs.Trips;
using Application.Interfaces.TripWhatToBrings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.TripWhatToBrings
{
    [Route("api/trips/{tripId:guid}/what-to-brings")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class TripWhatToBringsController : ControllerBase
    {
        private readonly IAddTripWhatToBringService _addTripWhatToBringService;
        private readonly IGetTripWhatToBringsService _getTripWhatToBringsService;
        private readonly IGetTripWhatToBringByIdService _getTripWhatToBringByIdService;
        private readonly IUpdateTripWhatToBringService _updateTripWhatToBringService;
        private readonly IDeleteTripWhatToBringService _deleteTripWhatToBringService;

        public TripWhatToBringsController(
            IAddTripWhatToBringService addTripWhatToBringService,
            IGetTripWhatToBringsService getTripWhatToBringsService,
            IGetTripWhatToBringByIdService getTripWhatToBringByIdService,
            IUpdateTripWhatToBringService updateTripWhatToBringService,
            IDeleteTripWhatToBringService deleteTripWhatToBringService)
        {
            _addTripWhatToBringService = addTripWhatToBringService;
            _getTripWhatToBringsService = getTripWhatToBringsService;
            _getTripWhatToBringByIdService = getTripWhatToBringByIdService;
            _updateTripWhatToBringService = updateTripWhatToBringService;
            _deleteTripWhatToBringService = deleteTripWhatToBringService;
        }

        [HttpPost]
        public async Task<ActionResult<TripWhatToBringAddedResponseDto>> AddTripWhatToBring(
            Guid tripId,
            [FromBody] CreateTripWhatToBringDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _addTripWhatToBringService.AddTripWhatToBringAsync(tripId, dto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpGet]
        public async Task<ActionResult<List<TripWhatToBringResponseDto>>> GetTripWhatToBrings(
            Guid tripId,
            CancellationToken cancellationToken)
        {
            var result = await _getTripWhatToBringsService.GetTripWhatToBringsAsync(tripId, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{whatToBringId:guid}")]
        public async Task<ActionResult<TripWhatToBringResponseDto>> GetTripWhatToBringById(
            Guid tripId,
            Guid whatToBringId,
            CancellationToken cancellationToken)
        {
            var result = await _getTripWhatToBringByIdService.GetTripWhatToBringByIdAsync(
                tripId, whatToBringId, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{whatToBringId:guid}")]
        public async Task<ActionResult<TripWhatToBringUpdatedResponseDto>> UpdateTripWhatToBring(
            Guid tripId,
            Guid whatToBringId,
            [FromBody] UpdateTripWhatToBringDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _updateTripWhatToBringService.UpdateTripWhatToBringAsync(
                tripId, whatToBringId, dto, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{whatToBringId:guid}")]
        public async Task<ActionResult<TripWhatToBringDeletedResponseDto>> DeleteTripWhatToBring(
            Guid tripId,
            Guid whatToBringId,
            CancellationToken cancellationToken)
        {
            var result = await _deleteTripWhatToBringService.DeleteTripWhatToBringAsync(
                tripId, whatToBringId, cancellationToken);
            return Ok(result);
        }
    }
}