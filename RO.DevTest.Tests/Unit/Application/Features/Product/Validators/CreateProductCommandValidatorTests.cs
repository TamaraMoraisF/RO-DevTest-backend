using FluentValidation.TestHelper;
using RO.DevTest.Application.Features.Product.Commands.CreateProductCommand;

namespace RO.DevTest.Tests.Unit.Application.Features.Product.Validators;

public class CreateProductCommandValidatorTests
{
    private readonly CreateProductCommandValidator _validator;

    public CreateProductCommandValidatorTests()
    {
        _validator = new CreateProductCommandValidator();
    }

    [Fact(DisplayName = "Should not have validation errors when data is valid")]
    public void Should_NotHaveValidationErrors_WhenDataIsValid()
    {
        var command = new CreateProductCommand { Name = "Monitor", Price = 599.99M };
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Should have validation error when name is empty")]
    public void Should_HaveValidationError_WhenNameIsEmpty()
    {
        var command = new CreateProductCommand { Name = "", Price = 100 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact(DisplayName = "Should have validation error when price is negative")]
    public void Should_HaveValidationError_WhenPriceIsNegative()
    {
        var command = new CreateProductCommand { Name = "Produto", Price = -10 };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }
}