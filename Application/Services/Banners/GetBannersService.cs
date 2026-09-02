using Application.DTOs.Banners;
using Application.DTOs.Common;
using Application.Interfaces.Banners;
using Application.Interfaces.IUnitOfWork;

namespace Application.Services.Banners
{
    public class GetBannersService : IGetBannersService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetBannersService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<BannerSummaryDto>> GetBannersAsync(GetBannersQueryDto query, CancellationToken cancellationToken)
        {
            return await _unitOfWork.Banners.GetBannersAsync(query, cancellationToken);
        }
    }
}