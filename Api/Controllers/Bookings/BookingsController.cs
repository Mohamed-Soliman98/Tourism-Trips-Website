using Application.DTOs.BookingInquiries;
using Application.DTOs.Common;
using Application.Interfaces.BookingInquiries;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace Api.Controllers.Bookings
{
    [Route("api/bookings")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class BookingsController : ControllerBase
    {
        private readonly ICreateBookingInquiryService _createBookingInquiryService;
        private readonly IGetBookingInquiriesService _getBookingInquiriesService;
        private readonly IGetBookingInquiryByIdService _getBookingInquiryByIdService;
        private readonly IUpdateBookingInquiryService _updateBookingInquiryService;
        private readonly IDeleteBookingInquiryService _deleteBookingInquiryService;

        public BookingsController(
            ICreateBookingInquiryService createBookingInquiryService,
            IGetBookingInquiriesService getBookingInquiriesService,
            IGetBookingInquiryByIdService getBookingInquiryByIdService,
            IUpdateBookingInquiryService updateBookingInquiryService,
            IDeleteBookingInquiryService deleteBookingInquiryService)
        {
            _createBookingInquiryService = createBookingInquiryService;
            _getBookingInquiriesService = getBookingInquiriesService;
            _getBookingInquiryByIdService = getBookingInquiryByIdService;
            _updateBookingInquiryService = updateBookingInquiryService;
            _deleteBookingInquiryService = deleteBookingInquiryService;
        }

        /// <summary>
        /// Public – Submit a booking inquiry.
        /// </summary>
        [HttpPost]
        [AllowAnonymous]
        [EnableRateLimiting("BookingInquiryPolicy")]
        [ProducesResponseType(typeof(BookingInquiryCreatedResponseDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status429TooManyRequests)]
        public async Task<ActionResult<BookingInquiryCreatedResponseDto>> CreateBookingInquiry(
            [FromBody] CreateBookingInquiryDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _createBookingInquiryService.CreateBookingInquiryAsync(dto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        /// <summary>
        /// Admin – Get paginated, filtered list of booking inquiries.
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(PagedResult<BookingInquirySummaryDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<ActionResult<PagedResult<BookingInquirySummaryDto>>> GetBookingInquiries(
            [FromQuery] GetBookingInquiriesQueryDto query,
            CancellationToken cancellationToken)
        {
            var result = await _getBookingInquiriesService.GetBookingInquiriesAsync(query, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Admin – Get a single booking inquiry by Id.
        /// </summary>
        [HttpGet("{id:guid}")]
        [ProducesResponseType(typeof(BookingInquiryDetailDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingInquiryDetailDto>> GetBookingInquiryById(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _getBookingInquiryByIdService.GetBookingInquiryByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Admin – Update booking inquiry status and/or internal notes.
        /// </summary>
        [HttpPut("{id:guid}")]
        [ProducesResponseType(typeof(BookingInquiryUpdatedResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingInquiryUpdatedResponseDto>> UpdateBookingInquiry(
            [FromRoute] Guid id,
            [FromBody] UpdateBookingInquiryDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _updateBookingInquiryService.UpdateBookingInquiryAsync(id, dto, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Admin – Delete a booking inquiry.
        /// </summary>
        [HttpDelete("{id:guid}")]
        [ProducesResponseType(typeof(BookingInquiryDeletedResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<BookingInquiryDeletedResponseDto>> DeleteBookingInquiry(
            [FromRoute] Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _deleteBookingInquiryService.DeleteBookingInquiryAsync(id, cancellationToken);
            return Ok(result);
        }
    }
}
