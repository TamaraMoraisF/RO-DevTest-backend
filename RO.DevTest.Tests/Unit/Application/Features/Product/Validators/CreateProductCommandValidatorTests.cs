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
        // Arrange
        var command = new CreateProductCommand { Name = "Monitor", Price = 599.99M };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Should have validation error when name is empty")]
    public void Should_HaveValidationError_WhenNameIsEmpty()
    {
        // Arrange
        var command = new CreateProductCommand { Name = "", Price = 100 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact(DisplayName = "Should have validation error when price is negative")]
    public void Should_HaveValidationError_WhenPriceIsNegative()
    {
        // Arrange
        var command = new CreateProductCommand { Name = "Produto", Price = -10 };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }
}
