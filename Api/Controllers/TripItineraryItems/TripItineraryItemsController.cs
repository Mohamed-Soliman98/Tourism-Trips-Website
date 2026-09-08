using Application.DTOs.TripItineraryItems;
using Application.DTOs.Trips;
using Application.Interfaces.TripItineraryItems;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.TripItineraryItems
{
    [Route("api/trips/{tripId:guid}/itinerary-items")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class TripItineraryItemsController : ControllerBase
    {
        private readonly IAddTripItineraryItemService _addTripItineraryItemService;
        private readonly IGetTripItineraryItemsService _getTripItineraryItemsService;
        private readonly IGetTripItineraryItemByIdService _getTripItineraryItemByIdService;
        private readonly IUpdateTripItineraryItemService _updateTripItineraryItemService;
        private readonly IDeleteTripItineraryItemService _deleteTripItineraryItemService;

        public TripItineraryItemsController(
            IAddTripItineraryItemService addTripItineraryItemService,
            IGetTripItineraryItemsService getTripItineraryItemsService,
            IGetTripItineraryItemByIdService getTripItineraryItemByIdService,
            IUpdateTripItineraryItemService updateTripItineraryItemService,
            IDeleteTripItineraryItemService deleteTripItineraryItemService)
        {
            _addTripItineraryItemService = addTripItineraryItemService;
            _getTripItineraryItemsService = getTripItineraryItemsService;
            _getTripItineraryItemByIdService = getTripItineraryItemByIdService;
            _updateTripItineraryItemService = updateTripItineraryItemService;
            _deleteTripItineraryItemService = deleteTripItineraryItemService;
        }

        [HttpPost]
        public async Task<ActionResult<TripItineraryItemAddedResponseDto>> AddTripItineraryItem(
            Guid tripId,
            [FromBody] CreateTripItineraryItemDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _addTripItineraryItemService.AddTripItineraryItemAsync(tripId, dto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpGet]
        public async Task<ActionResult<List<TripItineraryItemResponseDto>>> GetTripItineraryItems(
            Guid tripId,
            CancellationToken cancellationToken)
        {
            var result = await _getTripItineraryItemsService.GetTripItineraryItemsAsync(tripId, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{itineraryItemId:guid}")]
        public async Task<ActionResult<TripItineraryItemResponseDto>> GetTripItineraryItemById(
            Guid tripId,
            Guid itineraryItemId,
            CancellationToken cancellationToken)
        {
            var result = await _getTripItineraryItemByIdService.GetTripItineraryItemByIdAsync(
                tripId, itineraryItemId, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{itineraryItemId:guid}")]
        public async Task<ActionResult<TripItineraryItemUpdatedResponseDto>> UpdateTripItineraryItem(
            Guid tripId,
            Guid itineraryItemId,
            [FromBody] UpdateTripItineraryItemDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _updateTripItineraryItemService.UpdateTripItineraryItemAsync(
                tripId, itineraryItemId, dto, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{itineraryItemId:guid}")]
        public async Task<ActionResult<TripItineraryItemDeletedResponseDto>> DeleteTripItineraryItem(
            Guid tripId,
            Guid itineraryItemId,
            CancellationToken cancellationToken)
        {
            var result = await _deleteTripItineraryItemService.DeleteTripItineraryItemAsync(
                tripId, itineraryItemId, cancellationToken);
            return Ok(result);
        }
    }
}