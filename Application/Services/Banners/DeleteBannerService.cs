using Application.DTOs.Banners;
using Application.Interfaces.Banners;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;

namespace Application.Services.Banners
{
    public class DeleteBannerService : IDeleteBannerService
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBannerService(
            IUnitOfWork unitOfWork,
            IBannerRepository bannerRepository)
        {
            _unitOfWork = unitOfWork;
            _bannerRepository = bannerRepository;
        }

        public async Task<BannerDeletedResponseDto?> DeleteBannerAsync(Guid id, CancellationToken cancellationToken)
        {
            var banner = await _bannerRepository.GetByIdAsync(id, cancellationToken);
            
            if (banner == null)
                return null;

            _bannerRepository.Remove(banner);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new BannerDeletedResponseDto(
                id,
                "Banner deleted successfully."
            );
        }
    }
}