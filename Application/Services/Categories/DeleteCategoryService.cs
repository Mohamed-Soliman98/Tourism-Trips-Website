using Application.DTOs.Categories;
using Application.Interfaces.Categories;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;

namespace Application.Services.Categories
{
    public class DeleteCategoryService : IDeleteCategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryService(
            IUnitOfWork unitOfWork,
            ICategoryRepository categoryRepository)
        {
            _unitOfWork = unitOfWork;
            _categoryRepository = categoryRepository;
        }

        public async Task<CategoryDeletedResponseDto> DeleteCategoryAsync( Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Category Id cannot be empty.", nameof(id));
            }

            var category = await _categoryRepository.GetByIdAsync(id, cancellationToken);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID '{id}' was not found.");
            }

            _categoryRepository.Remove(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CategoryDeletedResponseDto();
        }
    }
}
