using Application.DTOs.Common;
using Application.DTOs.Testimonials;
using Application.Interfaces.Testimonials;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Testimonials
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class TestimonialsController : ControllerBase
    {
        private readonly ICreateTestimonialService _createTestimonialService;
        private readonly IGetTestimonialsService _getTestimonialsService;
        private readonly IGetTestimonialByIdService _getTestimonialByIdService;
        private readonly IUpdateTestimonialService _updateTestimonialService;
        private readonly IDeleteTestimonialService _deleteTestimonialService;

        public TestimonialsController(
            ICreateTestimonialService createTestimonialService,
            IGetTestimonialsService getTestimonialsService,
            IGetTestimonialByIdService getTestimonialByIdService,
            IUpdateTestimonialService updateTestimonialService,
            IDeleteTestimonialService deleteTestimonialService)
        {
            _createTestimonialService = createTestimonialService;
            _getTestimonialsService = getTestimonialsService;
            _getTestimonialByIdService = getTestimonialByIdService;
            _updateTestimonialService = updateTestimonialService;
            _deleteTestimonialService = deleteTestimonialService;
        }

        [HttpPost]
        public async Task<ActionResult<TestimonialCreatedResponseDto>> CreateTestimonial(
            [FromBody] CreateTestimonialDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _createTestimonialService.CreateTestimonialAsync(dto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpGet]
        public async Task<ActionResult<PagedResult<TestimonialSummaryDto>>> GetTestimonials(
            [FromQuery] GetTestimonialsQueryDto query,
            CancellationToken cancellationToken)
        {
            var result = await _getTestimonialsService.GetTestimonialsAsync(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<TestimonialDetailDto>> GetTestimonialById(
            Guid id,
            CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid testimonial ID.");
            }

            var result = await _getTestimonialByIdService.GetTestimonialByIdAsync(id, cancellationToken);
            
            if (result == null)
            {
                return NotFound($"Testimonial with ID {id} not found.");
            }

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<TestimonialUpdatedResponseDto>> UpdateTestimonial(
            Guid id,
            [FromBody] UpdateTestimonialDto dto,
            CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid testimonial ID.");
            }

            var result = await _updateTestimonialService.UpdateTestimonialAsync(id, dto, cancellationToken);
            
            if (result == null)
            {
                return NotFound($"Testimonial with ID {id} not found.");
            }

            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<TestimonialDeletedResponseDto>> DeleteTestimonial(
            Guid id,
            CancellationToken cancellationToken)
        {
            if (id == Guid.Empty)
            {
                return BadRequest("Invalid testimonial ID.");
            }

            var result = await _deleteTestimonialService.DeleteTestimonialAsync(id, cancellationToken);
            
            if (result == null)
            {
                return NotFound($"Testimonial with ID {id} not found.");
            }

            return Ok(result);
        }
    }
}