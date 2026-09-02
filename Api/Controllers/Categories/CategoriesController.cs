using Application.DTOs.Categories;
using Application.DTOs.Common;
using Application.Interfaces.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.Categories
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Admin")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICreateCategoryService _createCategoryService;
        private readonly IGetCategoriesService _getCategoriesService;
        private readonly IGetCategoryByIdService _getCategoryByIdService;
        private readonly IUpdateCategoryService _updateCategoryService;
        private readonly IDeleteCategoryService _deleteCategoryService;
        private readonly IGetPublicCategoriesService _getPublicCategoriesService;

        public CategoriesController(
            ICreateCategoryService createCategoryService,
            IGetCategoriesService getCategoriesService,
            IGetCategoryByIdService getCategoryByIdService,
            IUpdateCategoryService updateCategoryService,
            IDeleteCategoryService deleteCategoryService,
            IGetPublicCategoriesService getPublicCategoriesService)
        {
            _createCategoryService = createCategoryService;
            _getCategoriesService = getCategoriesService;
            _getCategoryByIdService = getCategoryByIdService;
            _updateCategoryService = updateCategoryService;
            _deleteCategoryService = deleteCategoryService;
            _getPublicCategoriesService = getPublicCategoriesService;
        }

        [HttpPost]
        public async Task<ActionResult<CategoryCreatedResponseDto>> CreateCategory(
            [FromBody] CreateCategoryDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _createCategoryService.CreateCategoryAsync(dto, cancellationToken);
            return StatusCode(StatusCodes.Status201Created, result);
        }

        [HttpGet]

        public async Task<ActionResult<PagedResult<CategorySummaryDto>>> GetCategories(
            [FromQuery] GetCategoriesQueryDto query,
            CancellationToken cancellationToken)
        {
            var result = await _getCategoriesService.GetCategoriesAsync(query, cancellationToken);
            return Ok(result);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<CategoryDetailDto>> GetCategoryById(
            Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _getCategoryByIdService.GetCategoryByIdAsync(id, cancellationToken);
            return Ok(result);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<CategoryUpdatedResponseDto>> UpdateCategory(
            Guid id,
            [FromBody] UpdateCategoryDto dto,
            CancellationToken cancellationToken)
        {
            var result = await _updateCategoryService.UpdateCategoryAsync(id, dto, cancellationToken);
            return Ok(result);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<CategoryDeletedResponseDto>> DeleteCategory(
            Guid id,
            CancellationToken cancellationToken)
        {
            var result = await _deleteCategoryService.DeleteCategoryAsync(id, cancellationToken);
            return Ok(result);
        }

        /// <summary>
        /// Public – Get all active categories for website navigation and filters.
        /// </summary>
        [HttpGet("public")]
        [AllowAnonymous]
        public async Task<ActionResult<List<PublicCategoryDto>>> GetPublicCategories(CancellationToken cancellationToken)
        {
            var result = await _getPublicCategoriesService.GetPublicCategoriesAsync(cancellationToken);
            return Ok(result);
        }
    }
}

