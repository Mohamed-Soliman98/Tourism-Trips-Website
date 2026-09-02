using Application.DTOs.Common;
using Application.DTOs.Destinations;
using Application.Interfaces.Destinations;
using Application.Interfaces.IUnitOfWork;
using Domain.Entity;
using FluentValidation;

namespace Application.Services.Destinations
{
    public class GetDestinationsService : IGetDestinationsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<GetDestinationsQueryDto> _validator;

        public GetDestinationsService(
            IUnitOfWork unitOfWork,
            IValidator<GetDestinationsQueryDto> validator)
        {
            _unitOfWork = unitOfWork;
            _validator = validator;
        }

        public async Task<PagedResult<DestinationSummaryDto>> GetDestinationsAsync(
            GetDestinationsQueryDto query,
            CancellationToken cancellationToken = default)
        {
            await _validator.ValidateAndThrowAsync(query, cancellationToken);

            var (items, totalCount) = await _unitOfWork.Destinations.GetDestinationsAsync(query, cancellationToken);

            var dtos = items.Select(MapToDto).ToList();

            return new PagedResult<DestinationSummaryDto>(
                dtos,
                query.Page,
                query.PageSize,
                totalCount
            );
        }

        private static DestinationSummaryDto MapToDto(Destination destination)
        {
            return new DestinationSummaryDto(
                destination.Id,
                destination.Name,
                destination.IsActive
            );
        }
    }
}
