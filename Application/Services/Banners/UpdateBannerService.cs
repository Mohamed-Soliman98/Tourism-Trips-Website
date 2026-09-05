using Application.DTOs.Banners;
using Application.Interfaces.Banners;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;

namespace Application.Services.Banners
{
    public class UpdateBannerService : IUpdateBannerService
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateBannerService(
            IUnitOfWork unitOfWork,
            IBannerRepository bannerRepository)
        {
            _unitOfWork = unitOfWork;
            _bannerRepository = bannerRepository;
        }

        public async Task<BannerUpdatedResponseDto?> UpdateBannerAsync(Guid id, UpdateBannerDto dto, CancellationToken cancellationToken)
        {
            var banner = await _bannerRepository.GetByIdAsync(id, cancellationToken);
            
            if (banner == null)
                return null;

            banner.Title = dto.Title;
            banner.Description = dto.Description;
            banner.ImageUrl = dto.ImageUrl;
            banner.ButtonText = dto.ButtonText;
            banner.ButtonUrl = dto.ButtonUrl;
            banner.DisplayOrder = dto.DisplayOrder;
            banner.IsActive = dto.IsActive;
            banner.UpdatedAt = DateTime.UtcNow;

            _bannerRepository.Update(banner);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new BannerUpdatedResponseDto(
                banner.Id,
                banner.Title,
                banner.Description,
                banner.ImageUrl,
                banner.ButtonText,
                banner.ButtonUrl,
                banner.DisplayOrder,
                banner.IsActive,
                banner.UpdatedAt.Value
            );
        }
    }
}