using Application.DTOs.CMSSections;
using Application.Interfaces.CMSSections;
using Application.Interfaces.IUnitOfWork;

namespace Application.Services.CMSSections
{
    public class DeleteCMSSectionService : IDeleteCMSSectionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCMSSectionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<CMSSectionDeletedResponseDto?> DeleteCMSSectionAsync(Guid id, CancellationToken cancellationToken)
        {
            var cmsSection = await _unitOfWork.CMSSections.GetByIdAsync(id, cancellationToken);
            
            if (cmsSection == null)
                return null;

            _unitOfWork.CMSSections.Remove(cmsSection);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CMSSectionDeletedResponseDto(
                id,
                "CMS section deleted successfully."
            );
        }
    }
}