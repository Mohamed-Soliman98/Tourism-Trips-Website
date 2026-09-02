using Application.DTOs.TripImages;
using Application.DTOs.Trips;
using Application.Interfaces.TripImages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.TripImages
{
    /// <summary>
    /// Manages the image gallery for a specific Trip.
    /// All operations are admin-only; gallery images are distinct from Trip.CoverImage.
    /// </summary>
    [Route("api/trips/{tripId:guid}/images")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class TripImagesController : ControllerBase
    {
        private readonly IAddTripImageService _addTripImageService;
        private readonly IGetTripGalleryService _getTripGalleryService;
        private readonly IUpdateTripImageService _updateTripImageService;
        private readonly IDeleteTripImageService _deleteTripImageService;

        public TripImagesController(
            IAddTripImageService addTripImageService,
            IGetTripGalleryService getTripGalleryService,
            IUpdateTripImageService updateTripImageService,
            IDeleteTripImageService deleteTripImageService)
        {
            _addTripImageService    = addTripImageService;
            _getTripGalleryService  = getTripGalleryService;
            _updateTripImageService = updateTripImageService;
            _deleteTripImageService = deleteTripImageService;
        }

        /// <summary>
        /// Upload and add a new image to the trip gallery.
        /// </summary>
        /// <remarks>Send as multipart/form-data.</remarks>
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult<TripImageAddedResponseDto>> AddTripImage(
            Guid tripId,
            [FromForm] AddTripImageDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _addTripImageService.AddTripImageAsync(tripId, dto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        /// <summary>
        /// Get all images in the trip gallery, ordered by DisplayOrder.
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<TripImageDto>>> GetTripGallery(
            Guid tripId,
            CancellationToken cancellationToken)
        {
            var result = await _getTripGalleryService.GetTripGalleryAsync(tripId, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Update the metadata (AltText, DisplayOrder, IsCover) of a gallery image.
        /// </summary>
        [HttpPut("{imageId:guid}")]
        public async Task<ActionResult<TripImageUpdatedResponseDto>> UpdateTripImage(
            Guid tripId,
            Guid imageId,
            [FromBody] UpdateTripImageDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _updateTripImageService.UpdateTripImageAsync(tripId, imageId, dto, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Delete a gallery image and its associated file from storage.
        /// </summary>
        [HttpDelete("{imageId:guid}")]
        public async Task<ActionResult<TripImageDeletedResponseDto>> DeleteTripImage(
            Guid tripId,
            Guid imageId,
            CancellationToken cancellationToken)
        {
            var result = await _deleteTripImageService.DeleteTripImageAsync(tripId, imageId, cancellationToken);
            return Ok(result);
        }
    }
}
