using FluentValidation.TestHelper;
using RO.DevTest.Application.Features.Product.Commands.UpdateProductCommand;

namespace RO.DevTest.Tests.Unit.Application.Features.Product.Validators;

public class UpdateProductCommandValidatorTests
{
    private readonly UpdateProductCommandValidator _validator;

    public UpdateProductCommandValidatorTests()
    {
        _validator = new UpdateProductCommandValidator();
    }

    [Fact(DisplayName = "Should not have validation errors when data is valid")]
    public void Should_NotHaveValidationErrors_WhenDataIsValid()
    {
        // Arrange
        var command = new UpdateProductWithIdCommand
        {
            Id = Guid.NewGuid(),
            Name = "Notebook",
            Price = 2999.99M
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Should have error when Id is empty")]
    public void Should_HaveError_WhenIdIsEmpty()
    {
        // Arrange
        var command = new UpdateProductWithIdCommand
        {
            Id = Guid.Empty,
            Name = "Notebook",
            Price = 1999.99M
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact(DisplayName = "Should have error when Name is empty")]
    public void Should_HaveError_WhenNameIsEmpty()
    {
        // Arrange
        var command = new UpdateProductWithIdCommand
        {
            Id = Guid.NewGuid(),
            Name = "",
            Price = 100
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact(DisplayName = "Should have error when Price is negative")]
    public void Should_HaveError_WhenPriceIsNegative()
    {
        // Arrange
        var command = new UpdateProductWithIdCommand
        {
            Id = Guid.NewGuid(),
            Name = "Produto",
            Price = -5
        };

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }
}
