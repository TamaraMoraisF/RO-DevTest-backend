using FluentValidation;

namespace RO.DevTest.Application.Features.Sale.Commands.GetPagedSales;

public class GetSalesAnalyticsCommandValidator : AbstractValidator<GetSalesAnalyticsCommand>
{
    public GetSalesAnalyticsCommandValidator()
    {
        RuleFor(x => x.Start)
            .NotEmpty().WithMessage("Start date is required.");

        RuleFor(x => x.End)
            .NotEmpty().WithMessage("End date is required.");

        RuleFor(x => x)
            .Must(x => x.End >= x.Start)
            .WithMessage("End date must be greater than or equal to Start date.");
    }
}
