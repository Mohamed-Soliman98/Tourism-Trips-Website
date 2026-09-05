using Application.DTOs.Banners;
using Application.Interfaces.Banners;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.Repositories;
using Domain.Entity;

namespace Application.Services.Banners
{
    public class CreateBannerService : ICreateBannerService
    {
        private readonly IBannerRepository _bannerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateBannerService(
            IUnitOfWork unitOfWork,
            IBannerRepository bannerRepository)
        {
            _unitOfWork = unitOfWork;
            _bannerRepository = bannerRepository;
        }

        public async Task<BannerCreatedResponseDto> CreateBannerAsync(CreateBannerDto dto, CancellationToken cancellationToken)
        {
            var banner = new Banner
            {
                Id = Guid.NewGuid(),
                Title = dto.Title,
                Description = dto.Description,
                ImageUrl = dto.ImageUrl,
                ButtonText = dto.ButtonText,
                ButtonUrl = dto.ButtonUrl,
                DisplayOrder = dto.DisplayOrder,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null
            };

            _bannerRepository.Add(banner);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return new BannerCreatedResponseDto(
                banner.Id,
                banner.Title,
                banner.Description,
                banner.ImageUrl,
                banner.ButtonText,
                banner.ButtonUrl,
                banner.DisplayOrder,
                banner.IsActive,
                banner.CreatedAt
            );
        }
    }
}