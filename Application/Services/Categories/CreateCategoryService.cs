using Application.DTOs.Categories;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Categories;
using Domain.Entity;
using FluentValidation;

namespace Application.Services.Categories
{
    public class CreateCategoryService : ICreateCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateCategoryDto> _validator;

        public CreateCategoryService(
            IUnitOfWork unitOfWork,
            IValidator<CreateCategoryDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<CategoryCreatedResponseDto> CreateCategoryAsync(
            CreateCategoryDto dto,
            CancellationToken cancellationToken = default)
        {
            await _validator.ValidateAndThrowAsync(dto, cancellationToken);

            if (await _unitOfWork.Categories.ExistsByNameAsync(dto.Name, cancellationToken))
            {
                throw new InvalidOperationException($"Category with name '{dto.Name.Trim()}' already exists.");
            }

            var category = new Category
            {
                Id = Guid.NewGuid(),
                Name = dto.Name.Trim(),
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _unitOfWork.Categories.Add(category);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CategoryCreatedResponseDto(
                category.Id,
                category.Name,
                category.IsActive,
                "Category created successfully.");
        }
    }
}
