using FluentValidation;

namespace RO.DevTest.Application.Features.Sale.Commands.GetPagedSales;

public class GetPagedSalesCommandValidator : AbstractValidator<GetPagedSalesCommand>
{
    public GetPagedSalesCommandValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");
    }
}