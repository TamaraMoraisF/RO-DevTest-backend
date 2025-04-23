using FluentValidation;

namespace RO.DevTest.Application.Features.Product.Commands.CreateProductCommand;

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("The name field is required.");

        RuleFor(p => p.Price)
            .GreaterThanOrEqualTo(0).WithMessage("The price must be zero or positive.");
    }
}
