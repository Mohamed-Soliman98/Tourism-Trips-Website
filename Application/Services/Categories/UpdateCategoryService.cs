using Application.DTOs.Categories;
using Application.Interfaces.Categories;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using FluentValidation;

namespace Application.Services.Categories
{
    public class UpdateCategoryService : IUpdateCategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<UpdateCategoryDto> _validator;

        public UpdateCategoryService(
            IUnitOfWork unitOfWork,
            ICategoryRepository categoryRepository,
            IValidator<UpdateCategoryDto> validator)
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
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

            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID '{id}' was not found.");
            }

            if (await _categoryRepository.ExistsByNameExcludingIdAsync(dto.Name, id, cancellationToken))
            {
                throw new InvalidOperationException($"A category with the name '{dto.Name.Trim()}' already exists.");
            }

            category.Name = dto.Name.Trim();
            category.IsActive = dto.IsActive;
            category.UpdatedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CategoryUpdatedResponseDto(
                category.Id,
                category.Name,
                category.IsActive
            );
        }
    }
}
