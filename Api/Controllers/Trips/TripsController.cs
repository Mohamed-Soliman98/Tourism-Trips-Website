using Application.DTOs.Common;
using Application.DTOs.Trips;
using Application.Interfaces.Trips;
using Domain.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Trips
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class TripsController : ControllerBase
    {
        private readonly ICreateTripService _createTripService;
        private readonly IGetTripByIdService _getTripByIdService;
        private readonly IGetTripBySlugService _getTripBySlugService;
        private readonly IGetPublicTripsService _getPublicTripsService;
        private readonly IGetAdminTripsService _getAdminTripsService;
        private readonly IUpdateTripService _updateTripService;
        private readonly IDeleteTripService _deleteTripService;
        private readonly IPublishTripService _publishTripService;
        private readonly IUnpublishTripService _unpublishTripService;
        private readonly IDuplicateTripService _duplicateTripService;

        public TripsController(
            ICreateTripService createTripService,
            IGetTripByIdService getTripByIdService,
            IGetTripBySlugService getTripBySlugService,
            IGetPublicTripsService getPublicTripsService,
            IGetAdminTripsService getAdminTripsService,
            IUpdateTripService updateTripService,
            IDeleteTripService deleteTripService,
            IPublishTripService publishTripService,
            IUnpublishTripService unpublishTripService,
            IDuplicateTripService duplicateTripService)
        {
            _createTripService = createTripService;
            _getTripByIdService = getTripByIdService;
            _getTripBySlugService = getTripBySlugService;
            _getPublicTripsService = getPublicTripsService;
            _getAdminTripsService = getAdminTripsService;
            _updateTripService = updateTripService;
            _deleteTripService = deleteTripService;
            _publishTripService = publishTripService;
            _unpublishTripService = unpublishTripService;
            _duplicateTripService = duplicateTripService;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<PagedResult<PublicTripSummaryDto>>> GetPublicTrips([FromQuery] GetPublicTripsQueryDto query, CancellationToken cancellationToken)
        {
            var result = await _getPublicTripsService.GetPublicTripsAsync(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("admin")]
        public async Task<ActionResult<PagedResult<AdminTripSummaryDto>>> GetAdminTrips([FromQuery] GetAdminTripsQueryDto query, CancellationToken cancellationToken)
        {
            var result = await _getAdminTripsService.GetAdminTripsAsync(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<TripCreatedResponseDto>> CreateTrip([FromForm] CreateTripDto dto, CancellationToken cancellationToken)
        {
            var result = await _createTripService.CreateTripAsync(dto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<TripUpdatedResponseDto>> UpdateTrip([FromRoute] Guid id, [FromForm] UpdateTripDto dto, CancellationToken cancellationToken)
        {
            var result = await _updateTripService.UpdateTripAsync(id, dto, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<TripDeletedResponseDto>> DeleteTrip([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _deleteTripService.DeleteTripAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<TripDetailsResponseDto>> GetTripById([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            var result = await _getTripByIdService.GetTripByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpGet("slug/{slug}")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(TripDetailsResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TripDetailsResponseDto>> GetTripBySlug(
            [FromRoute] string slug, 
            [FromQuery] Language language, 
            CancellationToken cancellationToken)
        {
            var result = await _getTripBySlugService.GetTripBySlugAsync(slug, language, cancellationToken);
            return Ok(result);
        }

        [HttpPost("{tripId:guid}/publish")]
        public async Task<ActionResult<TripPublishedResponseDto>> PublishTrip([FromRoute] Guid tripId, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _publishTripService.PublishTripAsync(tripId, cancellationToken);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id:guid}/unpublish")]
        public async Task<ActionResult<TripUnpublishedResponseDto>> UnpublishTrip([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _unpublishTripService.UnpublishTripAsync(id, cancellationToken);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("{id:guid}/duplicate")]
        public async Task<ActionResult<TripDuplicatedResponseDto>> DuplicateTrip([FromRoute] Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _duplicateTripService.DuplicateTripAsync(id, cancellationToken);
                return StatusCode(StatusCodes.Status201Created, result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
