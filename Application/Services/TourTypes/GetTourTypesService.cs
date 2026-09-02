using Application.DTOs.Common;
using Application.DTOs.TourTypes;
using Application.Interfaces.IUnitOfWork;
using Application.Interfaces.TourTypes;
using Domain.Entity;
using FluentValidation;

namespace Application.Services.TourTypes
{
    public class GetTourTypesService : IGetTourTypesService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<GetTourTypesQueryDto> _validator;

        public GetTourTypesService(
            IUnitOfWork unitOfWork,
            IValidator<GetTourTypesQueryDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<PagedResult<TourTypeSummaryDto>> GetTourTypesAsync(
            GetTourTypesQueryDto query,
            CancellationToken cancellationToken = default)
        {
            await _validator.ValidateAndThrowAsync(query, cancellationToken);

            var (items, totalCount) = await _unitOfWork.TourTypes.GetTourTypesAsync(query, cancellationToken);

            var dtos = items.Select(MapToDto).ToList();

            return new PagedResult<TourTypeSummaryDto>(
                dtos,
                query.Page,
                query.PageSize,
                totalCount
            );
        }

        private static TourTypeSummaryDto MapToDto(TourType tourType)
        {
            return new TourTypeSummaryDto(
                tourType.Id,
                tourType.Name,
                tourType.IsActive
            );
        }
    }
}
