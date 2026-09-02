using Application.DTOs.Banners;
using Application.Interfaces.Banners;
using Application.Interfaces.IUnitOfWork;

namespace Application.Services.Banners
{
    public class DeleteBannerService : IDeleteBannerService
    {
        private readonly IUnitOfWork _unitOfWork;

        public DeleteBannerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BannerDeletedResponseDto?> DeleteBannerAsync(Guid id, CancellationToken cancellationToken)
        {
            var banner = await _unitOfWork.Banners.GetByIdAsync(id, cancellationToken);
            
            if (banner == null)
                return null;

            _unitOfWork.Banners.Remove(banner);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new BannerDeletedResponseDto(
                id,
                "Banner deleted successfully."
            );
        }
    }
}