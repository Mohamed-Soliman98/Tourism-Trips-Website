using Application.DTOs.Common;
using Application.DTOs.TourTypes;
using Application.Interfaces.Repositories;
using Application.Interfaces.TourTypes;
using Domain.Entity;
using FluentValidation;

namespace Application.Services.TourTypes
{
    public class GetTourTypesService : IGetTourTypesService
    {
        private readonly ITourTypeRepository _tourTypeRepository;
        private readonly IValidator<GetTourTypesQueryDto> _validator;

        public GetTourTypesService(
            ITourTypeRepository tourTypeRepository,
            IValidator<GetTourTypesQueryDto> validator)
        {
            _tourTypeRepository = tourTypeRepository;
            _validator = validator;
        }

        public async Task<PagedResult<TourTypeSummaryDto>> GetTourTypesAsync(
            GetTourTypesQueryDto query,
            CancellationToken cancellationToken = default)
        {
            await _validator.ValidateAndThrowAsync(query, cancellationToken);

            var (items, totalCount) = await _tourTypeRepository.GetTourTypesAsync(query, cancellationToken);

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
