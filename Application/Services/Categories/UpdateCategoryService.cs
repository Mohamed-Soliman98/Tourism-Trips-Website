using Application.DTOs.Categories;
using Application.Interfaces.Categories;
using Application.Interfaces.IUnitOfWork;
using FluentValidation;

namespace Application.Services.Categories
{
    public class UpdateCategoryService : IUpdateCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateCategoryDto> _validator;

        public UpdateCategoryService(
            IUnitOfWork unitOfWork,
            IValidator<UpdateCategoryDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<CategoryUpdatedResponseDto> UpdateCategoryAsync(
            Guid id,
            UpdateCategoryDto dto,
            CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Category Id cannot be empty.", nameof(id));
            }

            // 1. Validate DTO
            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            // 2. Load existing category — reuses Generic Repository: GetByIdAsync
            var category = await _unitOfWork.Categories.GetByIdAsync(id, cancellationToken);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID '{id}' was not found.");
            }

            // 3. Check name uniqueness (exclude current record)
            if (await _unitOfWork.Categories.ExistsByNameExcludingIdAsync(dto.Name, id, cancellationToken))
            {
                throw new InvalidOperationException($"A category with the name '{dto.Name.Trim()}' already exists.");
            }

            // 4. Apply changes
            category.Name = dto.Name.Trim();
            category.IsActive = dto.IsActive;
            category.UpdatedAt = DateTime.UtcNow;

            // 5. Persist
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CategoryUpdatedResponseDto(
                category.Id,
                category.Name,
                category.IsActive
            );
        }
    }
}
