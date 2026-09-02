using Application.DTOs.Categories;
using Application.Interfaces.Categories;
using Application.Interfaces.IUnitOfWork;

namespace Application.Services.Categories
{
    public class DeleteCategoryService : IDeleteCategoryService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCategoryService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CategoryDeletedResponseDto> DeleteCategoryAsync( Guid id, CancellationToken cancellationToken = default)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Category Id cannot be empty.", nameof(id));
            }

            // Load existing category — reuses Generic Repository: GetByIdAsync
            var category = await _unitOfWork.Categories.GetByIdAsync(id, cancellationToken);
            if (category == null)
            {
                throw new KeyNotFoundException($"Category with ID '{id}' was not found.");
            }

            // Remove via Generic Repository: Remove()
            _unitOfWork.Categories.Remove(category);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CategoryDeletedResponseDto();
        }
    }
}
