using Application.DTOs.Banners;
using Application.Interfaces.Banners;
using Application.Interfaces.IUnitOfWork;

namespace Application.Services.Banners
{
    public class GetBannerByIdService : IGetBannerByIdService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBannerByIdService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<BannerDetailDto?> GetBannerByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var banner = await _unitOfWork.Banners.GetByIdAsync(id, cancellationToken);
            
            if (banner == null)
                return null;

            return new BannerDetailDto(
                banner.Id,
                banner.Title,
                banner.Description,
                banner.ImageUrl,
                banner.ButtonText,
                banner.ButtonUrl,
                banner.DisplayOrder,
                banner.IsActive,
                banner.CreatedAt,
                banner.UpdatedAt
            );
        }
    }
}