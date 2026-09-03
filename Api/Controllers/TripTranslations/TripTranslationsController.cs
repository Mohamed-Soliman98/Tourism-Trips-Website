using Application.DTOs.TripTranslations;
using Application.DTOs.Trips;
using Application.Interfaces.TripTranslations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.TripTranslations
{
    [Route("api/trips/{tripId:guid}/translations")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class TripTranslationsController : ControllerBase
    {
        private readonly IAddTripTranslationService _addTripTranslationService;
        private readonly IGetTripTranslationsService _getTripTranslationsService;
        private readonly IGetTripTranslationByIdService _getTripTranslationByIdService;
        private readonly IUpdateTripTranslationService _updateTripTranslationService;
        private readonly IDeleteTripTranslationService _deleteTripTranslationService;

        public TripTranslationsController(
            IAddTripTranslationService addTripTranslationService,
            IGetTripTranslationsService getTripTranslationsService,
            IGetTripTranslationByIdService getTripTranslationByIdService,
            IUpdateTripTranslationService updateTripTranslationService,
            IDeleteTripTranslationService deleteTripTranslationService)
        {
            _addTripTranslationService = addTripTranslationService;
            _getTripTranslationsService = getTripTranslationsService;
            _getTripTranslationByIdService = getTripTranslationByIdService;
            _updateTripTranslationService = updateTripTranslationService;
            _deleteTripTranslationService = deleteTripTranslationService;
        }

        [HttpPost]
        public async Task<ActionResult<TripTranslationAddedResponseDto>> AddTripTranslation(
            Guid tripId,
            [FromBody] CreateTripTranslationDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _addTripTranslationService.AddTripTranslationAsync(tripId, dto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpGet]
        public async Task<ActionResult<List<TripTranslationResponseDto>>> GetTripTranslations(
            Guid tripId,
            CancellationToken cancellationToken)
        {
            var result = await _getTripTranslationsService.GetTripTranslationsAsync(tripId, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{translationId:guid}")]
        public async Task<ActionResult<TripTranslationResponseDto>> GetTripTranslationById(
            Guid tripId,
            Guid translationId,
            CancellationToken cancellationToken)
        {
            var result = await _getTripTranslationByIdService.GetTripTranslationByIdAsync(
                tripId, translationId, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{translationId:guid}")]
        public async Task<ActionResult<TripTranslationUpdatedResponseDto>> UpdateTripTranslation(
            Guid tripId,
            Guid translationId,
            [FromBody] UpdateTripTranslationDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _updateTripTranslationService.UpdateTripTranslationAsync(
                tripId, translationId, dto, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{translationId:guid}")]
        public async Task<ActionResult<TripTranslationDeletedResponseDto>> DeleteTripTranslation(
            Guid tripId,
            Guid translationId,
            CancellationToken cancellationToken)
        {
            var result = await _deleteTripTranslationService.DeleteTripTranslationAsync(
                tripId, translationId, cancellationToken);
            return Ok(result);
        }
    }
}
