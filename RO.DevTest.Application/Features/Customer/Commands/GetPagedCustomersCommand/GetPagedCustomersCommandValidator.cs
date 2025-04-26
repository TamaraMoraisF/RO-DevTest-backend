using FluentValidation;

namespace RO.DevTest.Application.Features.Customer.Commands.GetPagedCustomersCommand;

public class GetPagedCustomersCommandValidator : AbstractValidator<GetPagedCustomersCommand>
{
    public GetPagedCustomersCommandValidator()
    {
        RuleFor(c => c.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0.");

        RuleFor(c => c.PageSize)
            .InclusiveBetween(1, 100).WithMessage("PageSize must be between 1 and 100.");
    }
}