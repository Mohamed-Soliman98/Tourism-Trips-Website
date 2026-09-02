using Application.DTOs.Banners;
using Application.Interfaces.Banners;
using Application.Interfaces.IUnitOfWork;

namespace Application.Services.Banners
{
    public class GetPublicBannersService : IGetPublicBannersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetPublicBannersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<PublicBannerDto>> GetPublicBannersAsync(CancellationToken cancellationToken = default)
        {
            var banners = await _unitOfWork.Banners.GetActiveBannersAsync(cancellationToken);

            return banners.Select(b => new PublicBannerDto(
                b.Id,
                b.Title,
                b.Description,
                b.ImageUrl,
                b.ButtonText,
                b.ButtonUrl,
                b.DisplayOrder
            )).ToList();
        }
    }
}
