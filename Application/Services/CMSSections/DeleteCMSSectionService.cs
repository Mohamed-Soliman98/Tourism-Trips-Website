using Application.DTOs.CMSSections;
using Application.Interfaces.CMSSections;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;

namespace Application.Services.CMSSections
{
    public class DeleteCMSSectionService : IDeleteCMSSectionService
    {
        private readonly ICMSSectionRepository _cmsSectionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteCMSSectionService(
            IUnitOfWork unitOfWork,
            ICMSSectionRepository cmsSectionRepository)
        {
            _unitOfWork = unitOfWork;
            _cmsSectionRepository = cmsSectionRepository;
        }

        public async Task<CMSSectionDeletedResponseDto?> DeleteCMSSectionAsync(Guid id, CancellationToken cancellationToken)
        {
            var cmsSection = await _cmsSectionRepository.GetByIdAsync(id, cancellationToken);
            
            if (cmsSection == null)
                return null;

            _cmsSectionRepository.Remove(cmsSection);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new CMSSectionDeletedResponseDto(
                id,
                "CMS section deleted successfully."
            );
        }
    }
}