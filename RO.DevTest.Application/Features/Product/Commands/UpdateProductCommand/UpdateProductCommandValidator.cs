using FluentValidation;

namespace RO.DevTest.Application.Features.Product.Commands.UpdateProductCommand;

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductWithIdCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("The name field is required.");

        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("The ID field is required.");

        RuleFor(p => p.Price)
            .GreaterThanOrEqualTo(0).WithMessage("The price must be zero or positive.");
    }
}
