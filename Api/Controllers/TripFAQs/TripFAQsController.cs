using Application.DTOs.TripFAQs;
using Application.DTOs.Trips;
using Application.Interfaces.TripFAQs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.TripFAQs
{
    [Route("api/trips/{tripId:guid}/faqs")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class TripFAQsController : ControllerBase
    {
        private readonly IAddTripFAQService _addTripFAQService;
        private readonly IGetTripFAQsService _getTripFAQsService;
        private readonly IGetTripFAQByIdService _getTripFAQByIdService;
        private readonly IUpdateTripFAQService _updateTripFAQService;
        private readonly IDeleteTripFAQService _deleteTripFAQService;

        public TripFAQsController(
            IAddTripFAQService addTripFAQService,
            IGetTripFAQsService getTripFAQsService,
            IGetTripFAQByIdService getTripFAQByIdService,
            IUpdateTripFAQService updateTripFAQService,
            IDeleteTripFAQService deleteTripFAQService)
        {
            _addTripFAQService = addTripFAQService;
            _getTripFAQsService = getTripFAQsService;
            _getTripFAQByIdService = getTripFAQByIdService;
            _updateTripFAQService = updateTripFAQService;
            _deleteTripFAQService = deleteTripFAQService;
        }

        [HttpPost]
        public async Task<ActionResult<TripFAQAddedResponseDto>> AddTripFAQ(
            Guid tripId,
            [FromBody] CreateTripFAQDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _addTripFAQService.AddTripFAQAsync(tripId, dto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpGet]
        public async Task<ActionResult<List<TripFAQResponseDto>>> GetTripFAQs(Guid tripId,CancellationToken cancellationToken)
        {
            var result = await _getTripFAQsService.GetTripFAQsAsync(tripId, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{faqId:guid}")]
        public async Task<ActionResult<TripFAQResponseDto>> GetTripFAQById(Guid tripId, Guid faqId,CancellationToken cancellationToken)
        {
            var result = await _getTripFAQByIdService.GetTripFAQByIdAsync(tripId, faqId, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{faqId:guid}")]
        public async Task<ActionResult<TripFAQUpdatedResponseDto>> UpdateTripFAQ(
            Guid tripId,
            Guid faqId,
            [FromBody] UpdateTripFAQDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _updateTripFAQService.UpdateTripFAQAsync(
                tripId, faqId, dto, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{faqId:guid}")]
        public async Task<ActionResult<TripFAQDeletedResponseDto>> DeleteTripFAQ(
            Guid tripId,
            Guid faqId,
            CancellationToken cancellationToken)
        {
            var result = await _deleteTripFAQService.DeleteTripFAQAsync(
                tripId, faqId, cancellationToken);
            return Ok(result);
        }
    }
}