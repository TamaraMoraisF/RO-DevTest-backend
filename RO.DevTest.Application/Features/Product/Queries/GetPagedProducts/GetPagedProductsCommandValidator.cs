using FluentValidation;

namespace RO.DevTest.Application.Features.Product.Queries.GetPagedProducts;

public class GetPagedProductsCommandValidator : AbstractValidator<GetPagedProductsCommand>
{
    public GetPagedProductsCommandValidator()
    {
        RuleFor(p => p.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0.");

        RuleFor(p => p.PageSize)
            .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");
    }
}
