using Application.DTOs.Categories;
using FluentValidation;

namespace Application.Validators.Categories
{
    public class GetCategoriesQueryDtoValidator : AbstractValidator<GetCategoriesQueryDto>
    {
        public GetCategoriesQueryDtoValidator()
        {
            RuleFor(x => x.Page)
                .GreaterThanOrEqualTo(1).WithMessage("Page number must be at least 1.");

            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1).WithMessage("Page size must be at least 1.")
                .LessThanOrEqualTo(50).WithMessage("Page size cannot exceed 50 items per page.");

            RuleFor(x => x.Search)
                .MaximumLength(100).When(x => !string.IsNullOrEmpty(x.Search))
                .WithMessage("Search query cannot exceed 100 characters.");
        }
    }
}
