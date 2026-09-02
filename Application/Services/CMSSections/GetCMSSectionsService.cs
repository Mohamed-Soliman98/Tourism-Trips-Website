using Application.DTOs.CMSSections;
using Application.DTOs.Common;
using Application.Interfaces.CMSSections;
using Application.Interfaces.IUnitOfWork;

namespace Application.Services.CMSSections
{
    public class GetCMSSectionsService : IGetCMSSectionsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public GetCMSSectionsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<PagedResult<CMSSectionSummaryDto>> GetCMSSectionsAsync(GetCMSSectionsQueryDto query, CancellationToken cancellationToken)
        {
            return await _unitOfWork.CMSSections.GetCMSSectionsAsync(query, cancellationToken);
        }
    }
}