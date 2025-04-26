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
        var command = new UpdateProductWithIdCommand
        {
            Id = Guid.NewGuid(),
            Name = "Notebook",
            Price = 2999.99M
        };

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact(DisplayName = "Should have error when Id is empty")]
    public void Should_HaveError_WhenIdIsEmpty()
    {
        var command = new UpdateProductWithIdCommand
        {
            Id = Guid.Empty,
            Name = "Notebook",
            Price = 1999.99M
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id);
    }

    [Fact(DisplayName = "Should have error when Name is empty")]
    public void Should_HaveError_WhenNameIsEmpty()
    {
        var command = new UpdateProductWithIdCommand
        {
            Id = Guid.NewGuid(),
            Name = "",
            Price = 100
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact(DisplayName = "Should have error when Price is negative")]
    public void Should_HaveError_WhenPriceIsNegative()
    {
        var command = new UpdateProductWithIdCommand
        {
            Id = Guid.NewGuid(),
            Name = "Produto",
            Price = -5
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Price);
    }
}
